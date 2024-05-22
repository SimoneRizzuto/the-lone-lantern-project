using System.Linq;
using Godot;
using Godot.Collections;

namespace DialogueManagerRuntime;
public partial class DialogueBalloon : CanvasLayer
{
  [Export] public string NextAction = "ui_accept";
  [Export] public string SkipAction = "ui_cancel";


  private Control balloon;
  private RichTextLabel characterLabel;
  private TextureRect portrait;
  private RichTextLabel dialogueLabel;
  private VBoxContainer responsesMenu;

  private Resource resource;
  private Array<Variant> temporaryGameStates = new();
  private bool isWaitingForInput;
  private bool willHideBalloon;

  private DialogueLine dialogueLine;
  private DialogueLine DialogueLine
  {
    get => dialogueLine;
    set
    {
      isWaitingForInput = false;
      balloon.FocusMode = Control.FocusModeEnum.All;
      balloon.GrabFocus();

      if (value == null)
      {
        QueueFree();
        return;
      }

      dialogueLine = value;
      UpdateDialogue();
    }
  }


  public override void _Ready()
  {
    balloon = GetNode<Control>("%Balloon");
    characterLabel = GetNode<RichTextLabel>("%CharacterLabel");
    portrait = GetNode<TextureRect>("%Portrait");
    dialogueLabel = GetNode<RichTextLabel>("%DialogueLabel");
    responsesMenu = GetNode<VBoxContainer>("%ResponsesMenu");

    balloon.Hide();

    balloon.GuiInput += (@event) =>
    {
      if ((bool)dialogueLabel.Get("is_typing"))
      {
        bool mouseWasClicked = @event is InputEventMouseButton && (@event as InputEventMouseButton).ButtonIndex == MouseButton.Left && @event.IsPressed();
        bool skipButtonWasPressed = @event.IsActionPressed(SkipAction);
        if (mouseWasClicked || skipButtonWasPressed)
        {
          GetViewport().SetInputAsHandled();
          dialogueLabel.Call("skip_typing");
          return;
        }
      }

      if (!isWaitingForInput) return;
      if (dialogueLine.Responses.Count > 0) return;

      GetViewport().SetInputAsHandled();

      if (@event is InputEventMouseButton && @event.IsPressed() && (@event as InputEventMouseButton).ButtonIndex == MouseButton.Left)
      {
        Next(dialogueLine.NextId);
      }
      else if (@event.IsActionPressed(NextAction) && GetViewport().GuiGetFocusOwner() == balloon)
      {
        Next(dialogueLine.NextId);
      }
    };

    if (string.IsNullOrEmpty((string)responsesMenu.Get("next_action")))
    {
      responsesMenu.Set("next_action", NextAction);
    }
    responsesMenu.Connect("response_selected", Callable.From((DialogueResponse response) =>
    {
      Next(response.NextId);
    }));

    DialogueManager.Mutated += OnMutated;
  }


  public override void _ExitTree()
  {
    DialogueManager.Mutated -= OnMutated;
  }


  public override void _UnhandledInput(InputEvent @event)
  {
    // Only the balloon is allowed to handle input while it's showing
    GetViewport().SetInputAsHandled();
  }


  public async void Start(Resource dialogueResource, string title, Array<Variant> extraGameStates = null)
  {
    temporaryGameStates = extraGameStates ?? new Array<Variant>();
    isWaitingForInput = false;
    resource = dialogueResource;

    DialogueLine = await DialogueManager.GetNextDialogueLine(resource, title, temporaryGameStates);
  }


  public async void Next(string nextId)
  {
    DialogueLine = await DialogueManager.GetNextDialogueLine(resource, nextId, temporaryGameStates);
  }


  #region Helpers


  private async void UpdateDialogue()
  {
    if (!IsNodeReady())
    {
      await ToSignal(this, SignalName.Ready);
    }

    // Set up the character name
    characterLabel.Visible = !string.IsNullOrEmpty(dialogueLine.Character);
    
    var splitCharacterLine = dialogueLine.Character.Split('_');
    var character = splitCharacterLine.First();
    
    characterLabel.Text = Tr(character, "dialogue");
    
    var portraitPath = $"res://Scenes/DialogueManager/Portraits/{character}/{dialogueLine.Character}.png";
    if (ResourceLoader.Exists(portraitPath))
    {
      portrait.Texture = GD.Load<Texture2D>(portraitPath);
    }
    else
    {
      portrait.Texture = null;
    }

    // Set up the dialogue
    dialogueLabel.Hide();
    dialogueLabel.Set("dialogue_line", dialogueLine);

    // Set up the responses
    responsesMenu.Hide();
    responsesMenu.Set("responses", dialogueLine.Responses);

    // Type out the text
    balloon.Show();
    willHideBalloon = false;
    dialogueLabel.Show();
    if (!string.IsNullOrEmpty(dialogueLine.Text))
    {
      dialogueLabel.Call("type_out");
      await ToSignal(dialogueLabel, "finished_typing");
    }

    // Wait for input
    if (dialogueLine.Responses.Count > 0)
    {
      balloon.FocusMode = Control.FocusModeEnum.None;
      responsesMenu.Show();
    }
    else if (!string.IsNullOrEmpty(dialogueLine.Time))
    {
      float time = 0f;
      if (!float.TryParse(dialogueLine.Time, out time))
      {
        time = dialogueLine.Text.Length * 0.02f;
      }
      await ToSignal(GetTree().CreateTimer(time), "timeout");
      Next(dialogueLine.NextId);
    }
    else
    {
      isWaitingForInput = true;
      balloon.FocusMode = Control.FocusModeEnum.All;
      balloon.GrabFocus();
    }
  }


  #endregion


  #region signals


  private void OnMutated(Dictionary _mutation)
  {
    isWaitingForInput = false;
    willHideBalloon = true;
    GetTree().CreateTimer(0.1f).Timeout += () =>
    {
      if (willHideBalloon)
      {
        willHideBalloon = false;
        balloon.Hide();
      }
    };
  }


  #endregion
}
