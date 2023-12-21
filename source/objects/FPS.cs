public partial class FPS : Label
{
    int fpsValue = 0;
    Tween tween;

	// update()
	public override void _Process(double delta)	{
        if (tween != null)
            tween.Kill();

        tween = GetTree().CreateTween();   
        tween.TweenProperty(this, "fpsValue", (int)Engine.GetFramesPerSecond(), 0.25f);
        Text = ("FPS: " + fpsValue);
	}
}
