using System.Collections;

public class Food  {

	public int sum = 5;
	public int x, y;

	public Food(int i, int a)
	{
		x = i;
		y = a;
	}

	public void Take(Ant x)
	{
		sum--;
		x.goHome = true;
		x.backHome = x.myTrail;
		//x.myTrail.Clear ();
		x.backHome.Reverse ();
		foreach (var item in x.backHome) {
			UnityEngine.Debug.Log (item.x + "," + item.y);
		}
	}

	public void Die()
	{
		ACO_Manager.instance.foods.Remove (this);
	}
}
