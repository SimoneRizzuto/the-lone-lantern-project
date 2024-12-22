using Godot;

namespace TheLoneLanternProject.Scripts.Modules.DustCloud;
public partial class DustCloudModule : DustCloudNode
{
	/* Three components are required: DustCloud, DustCloudModule, and the PlayerDashingModule.
	 *
	 * - DustCloud is responsible for holding what animation is played. It will hold the AnimatedSprite2D, with all the dust animations.
	 *		- It hold commands for what to specifically trigger. Perhaps an interface or a partial class would be good.
	 *		- Once an animation has finished playing, it should delete itself.
	 * - DustCloudModule is responsible for spawning DustClouds from a particular trigger. It will exist on the Player node.
	 *		_ It will also have commands for what DustCloud animation to trigger. It will spawn a DustCloud outside the Player node.
	 * - PlayingDashingModule will call the DustCloudModule to play the desired animation.
	 */

	[Export] public PackedScene DustCloudPackedScene;
	
	public override void Play_DashCloud(Vector2? position = null)
	{
		SpawnDustCloud(position).Play_DashCloud();
	}
	
	private DustCloudNode SpawnDustCloud(Vector2? position = null)
	{
		var dustCloud = DustCloudPackedScene.Instantiate() as DustCloudNode;
		
		var notNull = dustCloud != null && position != null;
		if (notNull)
		{
			dustCloud.Position = position.Value;
		}
		
		GetTree().CurrentScene.AddChild(dustCloud);
		
		return dustCloud;
	}
}