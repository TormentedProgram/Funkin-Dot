using NLua;

public partial class LuaScripts : Node
{
	private Lua luaScript;

	// create()
	public override void _EnterTree() {
		luaScript = new Lua();
		luaScript.RegisterFunction("DebugPrint", this, typeof(LuaScripts).GetMethod("Print"));

        luaScript.DoFile(Paths.scripts("script"));
		(luaScript["create"] as LuaFunction)?.Call();

		(luaScript["createPost"] as LuaFunction)?.Call();
	}


	// update()
	public override void _Process(double delta)	{
 		(luaScript["update"] as LuaFunction)?.Call(5);

		(luaScript["updatePost"] as LuaFunction)?.Call(5);
	}

	public void Print(string message)
    {
        GD.Print(message);
    }
}
