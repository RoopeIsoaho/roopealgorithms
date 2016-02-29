using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ACO_Manager : MonoBehaviour {
	
	public static ACO_Manager instance;
	public GameObject init, gridPrefab, gridParent;
	public List<Ant> ants = new List<Ant>();
	public List<Pheromone> pheromones = new List<Pheromone>();
	public List<Food> foods = new List<Food>();
	public List<Food> foodRemoval = new List<Food>();
	public static int GRID_WIDTH = 80;
	public static int GRID_HEIGHT = 80;
	public MeshRenderer[,] grid = new MeshRenderer[GRID_WIDTH,GRID_HEIGHT];
	public bool[,] obstacles;// = new bool[GRID_WIDTH,GRID_HEIGHT];

	 Color clrEmpty = Color.white;
	Color clrAnt = Color.black;
	Color clrObstacle = Color.grey;
	Color clrFood = Color.red;

	void Awake()
	{
		instance = this;
	}
		

	// Use this for initialization
	void Start () {
		obstacles = new bool[GRID_WIDTH, GRID_HEIGHT];

		for (int i = 0; i < GRID_WIDTH; i++) {
			for (int a = 0; a < GRID_HEIGHT; a++) {
				//obstacles [i, a] = false;
				GameObject x = Instantiate (gridPrefab) as GameObject;
				x.transform.position = new Vector3 (init.transform.position.x + i, 
					init.transform.position.y ,
					init.transform.position.z - a);
				grid [i, a] = x.GetComponent<MeshRenderer> ();
				x.transform.SetParent (gridParent.transform);

				if (Random.Range(0,40)<1)
				{
					obstacles [i, a] = true;
					if (Random.Range(0,2) < 0.2f)
					{
						if (i>0 && a > 0 && i<GRID_WIDTH-2 && a <GRID_HEIGHT-2)
						{
							obstacles [i - 1, a - 1] = true;
							obstacles [i + 1, a - 1] = true;
							obstacles [i - 1, a + 1] = true;
							obstacles [i + 1, a + 1] = true;
							obstacles [i, a -1] = true;
							obstacles [i, a +1] = true;
							obstacles [i-1, a ] = true;
							obstacles [i+1, a ] = true;
						}
					}
					grid [i, a].material.color = clrObstacle;
				}
			}
		}

		for (int i = 0; i < 20; i++) {
			foods.Add (new Food(Random.Range (0, GRID_WIDTH - 1), Random.Range (0, GRID_HEIGHT - 1)));
		}
		for (int i=0; i<4; i++)
		{
			int x = Mathf.RoundToInt (Random.Range (0, GRID_WIDTH));
			int y = Mathf.RoundToInt (Random.Range (0, GRID_HEIGHT));
			do {
				x = Mathf.RoundToInt (Random.Range (0, GRID_WIDTH));
				y = Mathf.RoundToInt (Random.Range (0, GRID_HEIGHT));
			} while(obstacles [x, y]);
			ants.Add (new Ant (x, y));
		}
		Destroy (init);
	}
	
	// Update is called once per frame
	void Update () {
		if ((System.DateTime.Now.Millisecond & 4)==0) {
			NextFrame ();
		}
	}




	void NextFrame()
	{
		foreach (var item in foodRemoval) {
			foods.Remove(item);
		}
		foodRemoval.Clear ();

		for (int i = 0; i < GRID_WIDTH; i++) {
			for (int a = 0; a < GRID_HEIGHT; a++) {
				grid [i, a].material.color = clrEmpty;
				if (obstacles[i,a])
				{
					grid [i, a].material.color = clrObstacle;
				}
			}
		}
		List<Pheromone> removal = new List<Pheromone> ();

		foreach (var item in pheromones) {
			item.Decay ();
			grid [item.x, item.y].material.color = clrEmpty - item.strength * Color.magenta;
			if (item.strength<0)
			{
				removal.Add (item);
			}
		}
		foreach (var item in foods) {
			grid [item.x, item.y].material.color = clrFood;
		}
		foreach (var item in ants) {
			AddPheromone (item.x, item.y, item);
			item.Move ();
			grid [item.x, item.y].material.color = clrAnt;
		}
		foreach (var item in removal) {
			pheromones.Remove (item);
		}



		foreach (var item in ants) {
			foreach (var item2 in foods) {
				if (Mathf.Abs(item.x-item2.x)+Mathf.Abs(item.y-item2.y) <= 2)
				{
					if (!item.goHome)
					{
						item2.Take (item);
						if (item2.sum <= 0)
						{
							foodRemoval.Add (item2);
						}
					}
				}
			}
		}

	}



	void AddPheromone(int x, int y, Ant a)
	{
		Pheromone c = null;
		foreach (var item in pheromones) {
			if (item.x == x)
			{
				if (item.y == y)
				{
					c = item;
					item.strength++;
				}
			}
		}
		if (c == null)
		{
			c = new Pheromone (x, y);
		}
		pheromones.Add (c);
		a.myTrail.Add (c);
		grid [x, y].GetComponent<MeshRenderer> ().material.color = clrEmpty - c.strength * Color.gray;
	}
}
