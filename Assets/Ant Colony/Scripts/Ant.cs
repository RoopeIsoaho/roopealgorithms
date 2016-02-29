using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ant {

	public bool goHome = false;
	public int x, y;
	public int targetX,targetY;
	public int tempTargetX,tempTargetY;
	public List<Pheromone> myTrail = new List<Pheromone>();
	public List<Pheromone> backHome = new List<Pheromone>();

	public Ant(int i, int a)
	{
		x = i;
		y = a;
		NewTempTarget (8);
	}

	public void NewTempTarget(int radius)
	{
		tempTargetX = x - radius + (int)Random.Range (0, radius*2);
		tempTargetY = y - radius + (int)Random.Range (0, radius*2);
		tempTargetX = Mathf.Clamp (tempTargetX, 0, ACO_Manager.GRID_WIDTH-1);
		tempTargetY = Mathf.Clamp (tempTargetY, 0, ACO_Manager.GRID_HEIGHT-1);
	}

	public void Move()
	{
		if (goHome) {
			Pheromone last = backHome[0];
			x = last.x;
			y = last.y;
			if (backHome.Count > 0)
			{
				backHome.Remove (backHome [0]);
			}
			else
			{
				goHome = false;
			}
		} else {
			int i = x;
			int a = y;
			bool b = true;
			do {
				b = true;
				x = i;
				y = a;
				if (Random.Range (0, 3) < 1) {
					x += Mathf.Clamp (x - tempTargetX, -1, 1);
					y += Mathf.Clamp (y - tempTargetY, -1, 1);
				} else {
					x += (int)Random.Range (-1, 2);
					y += (int)Random.Range (-1, 2);
				}
				int s = (int)x;
				int d = (int)y;
				if (s >= 0 && d >= 0 && s < ACO_Manager.GRID_WIDTH && d < ACO_Manager.GRID_HEIGHT) {
					b = ACO_Manager.instance.obstacles [s, d];
				}
			} while (b);
		}


		if (Mathf.Abs(tempTargetX-x)+Mathf.Abs(tempTargetY-y) < 2)
		{
			NewTempTarget (6);
		}
	}
}
