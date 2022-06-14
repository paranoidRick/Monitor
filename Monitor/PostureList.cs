namespace Monitor;

internal class PostureList
{
	private string posture;

	private string onbed;

	private string time;

	private int id;

	public string Posture
	{
		get
		{
			return posture;
		}
		set
		{
			posture = value;
		}
	}

	public string Onbed
	{
		get
		{
			return onbed;
		}
		set
		{
			onbed = value;
		}
	}

	public string Time
	{
		get
		{
			return time;
		}
		set
		{
			time = value;
		}
	}

	public int Id
	{
		get
		{
			return id;
		}
		set
		{
			id = value;
		}
	}
}
