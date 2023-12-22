public partial class FPS : Label
{
	int fpsValue = 0;

	float memInBytes = 0;
	float memPeakInBytes = 0;
	string mem = "0";

	string memPeak = "0";
	Tween tween;

	Timer memTimer;

    public override void _Ready()
    {
        memTimer = new Timer();
		AddChild(memTimer);
		memTimer.WaitTime = 0.5f;
		memTimer.OneShot = false;
		memTimer.Timeout += updateMemory;
		memTimer.Start();
    }

	private void updateMemory() {
		memInBytes = (int)GC.GetTotalMemory(false);
		if (memInBytes > memPeakInBytes) {
			memPeakInBytes = memInBytes;
		}
		mem = CoolUtil.formatMemory((int)GC.GetTotalMemory(false));
		memPeak = CoolUtil.formatMemory((int)memPeakInBytes);
	}

    // update()
    public override void _Process(double delta)	{
		if (tween != null)
			tween.Kill();

		tween = GetTree().CreateTween();   
		tween.SetParallel(true);
		tween.TweenProperty(this, "fpsValue", (int)Engine.GetFramesPerSecond(), 0.25f);
		Text = ($"FPS: {fpsValue}\nRAM: {mem} / {memPeak}\nVRAM: (WIP)");
	}
}
