using Assets.Scripts.Entities.Player;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{
    public float MoveSpeed = 5f;
    public float RotationSpeed = 700f;
    public Transform CameraPivot;

    [SerializeField]
    private PlayerInventory Inventory;
    private bool InventoryOpen = false;

    [SerializeField]
    private DevPanelUI _devPanelUI;
    private bool _devPanelOpen = false;

    private PlayerControls Controls;
    private CharacterController Controller;
    private PlayerInteraction Interaction;

    private Vector2 MoveInput;
    private Vector2 LookInput;


    private Animator animator;

    private float xRotation = 0f;

    void Awake()
    {
        Controller = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
        Interaction = GetComponent<PlayerInteraction>();

        if (Inventory == null)
            Inventory = GetComponent<PlayerInventory>();

        Controls = new PlayerControls();
        Controls.Player.Mover.performed += ctx => MoveInput = ctx.ReadValue<Vector2>();
        Controls.Player.Mover.canceled += ctx => MoveInput = Vector2.zero;

        Controls.Player.Look.performed += ctx => LookInput = ctx.ReadValue<Vector2>();
        Controls.Player.Look.canceled += ctx => LookInput = Vector2.zero;

        Controls.Player.Inventory.performed += ctx => ToggleInventory();
        Controls.Player.DevPanel.performed += ctx => ToggleDevPanel();

        Controls.Player.Interact.performed += ctx => Interaction.TryInteract();

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void ToggleInventory()
    {
        InventoryOpen = !InventoryOpen;

        if (InventoryOpen)
        {
            Inventory.OpenInventory();

            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;

            Controls.Player.Mover.Disable();
            Controls.Player.Look.Disable();
        }
        else
        {
            Inventory.CloseInventory();

            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;

            Controls.Player.Mover.Enable();
            Controls.Player.Look.Enable();
        }
    }

    private void ToggleDevPanel()
    {
        _devPanelOpen = !_devPanelOpen;

        if (_devPanelOpen)
        {
            _devPanelUI.Open();

            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;

            Controls.Player.Mover.Disable();
            Controls.Player.Look.Disable();
        }
        else
        {
            _devPanelUI.Close();

            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;

            Controls.Player.Mover.Enable();
            Controls.Player.Look.Enable();
        }
    }

    void OnEnable() => Controls.Enable();
    void OnDisable() => Controls.Disable();

    void Update()
    {
        MovePlayer();
        RotatePlayer();
    }

    private void MovePlayer()
    {
        Vector3 dir = new Vector3(MoveInput.x, 0, MoveInput.y);
        Vector3 moveDir = transform.forward * dir.z + transform.right * dir.x;

        Controller.Move(moveDir * MoveSpeed * Time.deltaTime);

        // Atualiza parâmetro do Animator
        float speed = new Vector3(moveDir.x, 0, moveDir.z).magnitude;
        animator.SetFloat("Speed", speed);
    }

    private void RotatePlayer()
    {
        // Rotação horizontal do player
        transform.Rotate(Vector3.up, LookInput.x * RotationSpeed * Time.deltaTime);

        // Rotação vertical do pivot da câmera
        xRotation -= LookInput.y * RotationSpeed * Time.deltaTime;
        xRotation = Mathf.Clamp(xRotation, -80f, 80f); // limitar rotação vertical
        CameraPivot.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
    }
}
