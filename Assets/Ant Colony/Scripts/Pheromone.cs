using System.Collections;

public class Pheromone  {

	public float strength = 1;
	public int x, y;

	public Pheromone(int i, int a)
	{
		x = i;
		y = a;
	}

	public void Decay()
	{
		strength -= 0.0001f;
		strength *= 0.96f;
	}
}
