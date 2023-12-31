
public partial class SparrowAnimation : Node
{
    private Timer animationTimer;
    private int currentRectIndex;
    private List<SpriteMeta> currentRects;
	private DynamicAnimationData currentData;

    public List<SpriteMeta> allRects;
    public List<DynamicAnimationData> allData = new List<DynamicAnimationData>();

    public string curAnim;
    public bool finished = false;

    // Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		animationTimer = new Timer();
		AddChild(animationTimer);
		animationTimer.Timeout += _OnAnimationTimerTimeout;
	}

    public void setpath(string path)
    {
        allRects = SparrowParser.ParseAsset(Paths.atlas(path));

		Image image = new Image();
        image.Load(checkExistLoc($"images/{path}.png"));
        Texture2D asset = ImageTexture.CreateFromImage(image);

		GetParent<Sprite2D>().Texture = asset;
    }

	public void create(string tag, string animName, float fps, bool loop)
	{
		// Check if the tag already exists in allData
		DynamicAnimationData existingAnim = allData.FirstOrDefault(data => data.name == tag);

		if (existingAnim != null)
		{
			// Update existing entry
			existingAnim.fps = fps;
			existingAnim.loop = loop;
			existingAnim.animRects = allRects
				.Where(spriteMeta => spriteMeta.name.Contains(animName))
				.ToList();
		}
		else
		{
			// Create a new entry
			List<SpriteMeta> currentRects = allRects
				.Where(spriteMeta => spriteMeta.name.Contains(animName))
				.ToList();

			DynamicAnimationData newAnim = new DynamicAnimationData();
			newAnim.name = tag;
			newAnim.fps = fps;
			newAnim.loop = loop;
			newAnim.animRects = currentRects;

			allData.Add(newAnim);
		}
	}

    public void centerOffsets() {
        foreach(DynamicAnimationData data in allData) {
            data.centerOffsets();
        }
    }

	public void play(string tag)
	{
		DynamicAnimationData data = GetAnimationData(tag);

		if (data != null)
		{
			currentData = data;

			float fps = data.fps;
			float frameDuration = 1.0f / fps;

			// Reset the timer and start it
			animationTimer.WaitTime = frameDuration;
			animationTimer.OneShot = false;
			animationTimer.Start();

			// Initialize the currentRectIndex
			currentRectIndex = 0;

			// Trigger the first frame
            finished = false;
            curAnim = tag;
			_OnAnimationTimerTimeout();  // Call the method directly here
		}
		else
		{
			//GD.Print("Animation data not found for tag: " + tag);
		}
	}

    private DynamicAnimationData GetAnimationData(string tag)
    {
        return allData
            .Where(animData => animData.name == tag)
            .FirstOrDefault();
    }

	private void _OnAnimationTimerTimeout()
	{
		List<SpriteMeta> currentRects = currentData.animRects;
		Sprite2D sprite = GetParent<Sprite2D>();

		// Check if there are more frames to display
		if (currentRectIndex < currentRects.Count)
		{
			// Update the sprite's RegionRect with the current frame
            sprite.Centered = false;
			sprite.RegionEnabled = true;
			sprite.RegionRect = currentRects[currentRectIndex].rect;
			sprite.Offset = currentRects[currentRectIndex].offset;

			// Move to the next frame
			currentRectIndex++;

			// Restart the timer for the next frame
			animationTimer.Start();
		}
		else
		{
			if (currentRectIndex >= currentRects.Count)
			{
				if (currentData.loop)
				{
					// Reset the index to loop
					currentRectIndex = 0;
				}
				else
				{
					// Animation has finished
                    finished = true;
					animationTimer.Stop();
					return;
				}
			}
		}
	}

}

public class DynamicAnimationData {
	public string name;
	public float fps = 24;
	public bool loop = false;
	public List<SpriteMeta> animRects;

    public void centerOffsets()
    {
        if (animRects.Count > 0)
        {
            Vector2 centerOffset = -animRects[0].rect.Size / 2.0f;

            foreach (var rect in animRects)
            {
                centerOffset = centerOffset.Lerp(-rect.rect.Size / 2.0f, 0.5f);
            }

            // Calculate the total offset to apply
            Vector2 totalOffset = centerOffset - animRects[0].offset;

            foreach (var rect in animRects)
            {
                rect.offset += totalOffset;
            }
        }
    }
}