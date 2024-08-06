using Godot;
using TheLoneLanternProject.Constants;

public partial class PauseMenu : Control
{
    public bool Playing = true;

    public override void _Ready()
    {
        Hide();
    }


    // I think that this should go somewhere more important maybe???
    public override void _Process(double delta)
    {
        if (Input.IsActionPressed(InputMapAction.Pause))
        {
            if (Playing)
            {
                GetTree().Paused = true;
                Playing = !Playing;
                Show();
                return;
                // Send signal to show or just show the pause menu UI which will have the resume button. 
                // Right now pressing this will be super annnoying and laggy when pausing and  unpausing. 
                // Having resume button will make it cleaner and not have conflicting messages. 
            }
            else
            {
                GetTree().Paused = false;
                Playing = !Playing;
                Hide();
                return;
            }


        }
    }

    public void OnResumePressed()
    {
        // Simply unpause and remove pause menu
        GetTree().Paused = false;
        Playing = !Playing;
        Hide();
    }

    public void OnExitPressed()
    {
        GetTree().Root.PropagateNotification((int)NotificationWMCloseRequest);

    }
    public override void _Notification(int what)
    {
        if (what == NotificationWMCloseRequest)
            GetTree().Quit();
    }


}
