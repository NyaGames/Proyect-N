using UnityEngine;
using Photon.Pun;

[RequireComponent(typeof(DropAnimation))]
public class Drop : MonoBehaviour
{
    public bool movingObject;
    public bool creatingObject;

	[SerializeField] private float pickUpRange = 5f;
	[SerializeField] private GameObject rangeScaler;

	[SerializeField] private Transform player;

	DropAnimation dropAnimation;
	bool pickable = false;

	private void Awake()
	{
		dropAnimation = GetComponent<DropAnimation>();

		if (!GameManager.Instance.myPlayer.GetComponent<Player>().isGameMaster)
		{
			pickable = true;
			player = GameManager.Instance.myPlayer.transform;
		}
	}

	private void OnValidate()
	{
		rangeScaler.transform.localScale = Vector3.one * (pickUpRange * 0.25f);
	}

	private void Start()
    {
        movingObject = false;

		rangeScaler.transform.localScale = Vector3.one * (pickUpRange * 0.25f);
	}

	private void Update()
    {
        if (!movingObject ) //Si no se mueve y no se esta creando, lo detectamos
        {
            detectObjectTapped();
        }
        else if (movingObject)  //Si puede moverse y no se esta crando, lo movemos
        {
            moveObject();
        }

		//Animaciones
		if (!pickable)
		{
			//GM
			if (!dropAnimation.activated)
			{

				for (int i = 0; i < GamemasterManager.Instance.playersViewsList.Length; i++)
				{
					
					GameObject player = GamemasterManager.Instance.playersViewsList[i];

					if (!player.GetPhotonView().Owner.IsMasterClient)
					{
						if (Vector3.Distance(player.transform.position, transform.position) <= pickUpRange)
						{
							dropAnimation.ActivateDrop();
						}
					}
				}
			}
			else
			{
				bool someoneInRange = false;
				for (int i = 0; i < GamemasterManager.Instance.playersViewsList.Length; i++)
				{
					GameObject player = GamemasterManager.Instance.playersViewsList[i];
					if (Vector3.Distance(player.transform.position, transform.position) <= pickUpRange)
					{
						someoneInRange = true;
					}
				}

				if (!someoneInRange)
				{
					dropAnimation.DeactivateDrop();
				}
			}
		}
		else
		{
			//Player
			if (!dropAnimation.activated)
			{
				if (Vector3.Distance(transform.position, player.transform.position) <= pickUpRange)
				{
					dropAnimation.ActivateDrop();
				}
			}
			else
			{
				if (Vector3.Distance(transform.position, player.transform.position) > pickUpRange)
				{
					dropAnimation.DeactivateDrop();					
				}
			}
		}
    }

    public void detectObjectTapped()
    {
        if ((Input.touchCount > 0) && (Input.GetTouch(0).phase == TouchPhase.Began))
        {
            Ray raycast = Camera.main.ScreenPointToRay(Input.GetTouch(0).position);
            RaycastHit raycastHit;
            if (Physics.Raycast(raycast, out raycastHit))
            {
                Debug.Log("Something Hit");
                if (raycastHit.collider.gameObject == this.gameObject)
                {					
                    movingObject = true;
                    Debug.Log("Drop tapped");
                }

				if (raycastHit.collider.tag.Equals("Drop"))
				{
					//dropAnimation.TryDestroyAnimation();
					if (!GameManager.Instance.myPlayer.GetPhotonView().Owner.IsMasterClient)
					{
						PickUpDrop();
					}
				}

            }
        }
    }
    public void moveObject()
    {
        if (Input.touchCount > 0)
        {           
            Touch touch = Input.GetTouch(0);
            float cameraDistanceToGround = GamemasterManager.Instance.getDistanceFromCameraToGround();
            Vector3 worldPosition = Camera.main.ScreenToWorldPoint(new Vector3(touch.position.x, touch.position.y, cameraDistanceToGround));
            worldPosition.y = 0;
            switch (touch.phase)
            {
                case TouchPhase.Moved:
                    this.gameObject.transform.position = worldPosition;
                    break;

                case TouchPhase.Ended:
                    movingObject = false;
                    break;
            }
        }
    }

	//Se está cerca de un drop y se ha pulsado, 
	public void PickUpDrop()
	{
		//TODO: Darle munición a este jugador y llamar a dropAnimation.TryDestroyAnimation en todos los clientes
	}	
}
