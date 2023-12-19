global using Godot;

global using System;
global using System.Collections.Generic;
global using System.IO;
global using System.Linq;
global using System.Text.RegularExpressions;

global using Newtonsoft.Json;
global using Newtonsoft.Json.Linq;

global using static EngineUtil;

using GodotUtils;

public partial class Main : Node
{
	public static ServiceProvider Services { get; } = new ServiceProvider();

	public override void _Ready()
	{
		GodotUtils.GU.Init(Services);

		//example
		//Main.Services.Add(this, persistent: false); 
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}
