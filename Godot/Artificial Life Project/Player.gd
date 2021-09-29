extends Actor
onready var cam:Camera = $Camera

var running:bool
var crouching:bool

func _ready():
	Input.set_mouse_mode(Input.MOUSE_MODE_CAPTURED)

func _input(event) -> void:
	if event is InputEventMouseMotion:
		if Input.get_mouse_mode() == Input.MOUSE_MODE_CAPTURED:
			var cam_rot:Vector2 = event.relative / -10.0
			cam.rotation_degrees.x = clamp(cam.rotation_degrees.x + cam_rot.y, -75, 75)
			cam.rotation_degrees.y += cam_rot.x
	
	if event.is_action_pressed("show_mouse"):
		if Input.get_mouse_mode() == Input.MOUSE_MODE_CAPTURED:
			Input.set_mouse_mode(Input.MOUSE_MODE_VISIBLE)
		elif Input.get_mouse_mode() == Input.MOUSE_MODE_VISIBLE:
			Input.set_mouse_mode(Input.MOUSE_MODE_CAPTURED)
	
	if event.is_action_pressed("run"):
		running = true
	if event.is_action_released("run"):
		running = false
	
	if event.is_action_pressed("crouch"):
		crouching = true
		hurtbox.height = 2.0
		find_node("CollisionShape").translation.y = 1
		cam.translation.y = 1.5
	if event.is_action_released("crouch"):
		crouching = false
		hurtbox.height = 3.0
		find_node("CollisionShape").translation.y = 1.5
		cam.translation.y = 2.5
	

func _physics_process(_delta)->void:
	space_state = get_world().direct_space_state
	
	movement_vector.x = (int(Input.is_action_pressed("move_right")) -
			int(Input.is_action_pressed("move_left")))
	movement_vector.z = (int(Input.is_action_pressed("move_backward")) -
			int(Input.is_action_pressed("move_forward")))
	
	movement_vector = cam.transform * movement_vector
	movement_vector.y = 0
	movement_vector = movement_vector.normalized()
	
	movement_vector *= (movement_speed * float(not running and not crouching)
			+ movement_speed * (float(running and not crouching) * 2.0)
			+ movement_speed * (float(crouching) * 0.5))
	
	movement_vector.y = -5
	
	_push_back()
	
	_check_walls()
	_check_floors()
	_check_ceilings()
	
	translation = delta_pos()
	
	movement_vector = Vector3()
