extends Area
class_name Actor

onready var collision_shape:CollisionShape = $CollisionShape
onready var hurtbox:CylinderShape

var movement_vector:Vector3
export var movement_speed:float = 1

var grounded:bool
var space_state:PhysicsDirectSpaceState

var wall_checks:Array

export var min_wall_height:float = 0.6

func _ready()->void:
	assert(collision_shape.shape.is_class("CylinderShape"))
	hurtbox = collision_shape.shape as CylinderShape
	
	var val:int = 8
	for theta in val:
		theta *= (TAU / val as float)
		wall_checks.append(Vector3(sin(theta) * (hurtbox.radius + 0.1), min_wall_height, cos(theta) * (hurtbox.radius + 0.1)))
	

func _process(_delta):
	pass

func _physics_process(_delta)->void:
	pass

func _check_walls()->void:   
	var result:Dictionary
	for i in 8:
		result = space_state.intersect_ray(
			delta_pos() + Vector3(0, min_wall_height, 0),
			delta_pos() + wall_checks[i],
			[], 1,true, false
			)
		
		if !result.empty():
			var angle_to:float = result.normal.angle_to(Vector3.UP)
			if angle_to > TAU/8.0 and angle_to <= (TAU / 3.6) + 0.03:
				var surface_normal:Vector3 = result.normal
				surface_normal.y = 0;
				
				if surface_normal.dot(movement_vector.normalized()) > 0:
					continue
				
				var cross:Vector3 = surface_normal.cross(Vector3.UP)
				var new_move:Vector3 = movement_vector
				new_move.y = 0
				
				var direction_to:Vector3 = delta_pos().direction_to(result.position - (Vector3.UP * min_wall_height))
				var distance_to:float = delta_pos().distance_to(result.position - (Vector3.UP * min_wall_height))
				if distance_to <= hurtbox.radius:
					
					var val:float = (1 / (hurtbox.radius * 2)) * distance_to
					val *= val
					new_move = direction_to * surface_normal.dot(direction_to) * (1-(val)) * 0.5
				else:
					new_move = new_move.project(cross)
				movement_vector.x = new_move.x
				movement_vector.z = new_move.z
		
		result.clear()
	

func _check_floors()->void:
	var result:Dictionary = space_state.intersect_ray(
		delta_pos() + Vector3.UP * 1.0,
		delta_pos(),
		[], 1, true, false
		)
	
	grounded = false
	
	if !result.empty():
		grounded = true
		var angle_to:float = result.normal.angle_to(Vector3.UP)
		if angle_to <= TAU/8:
			translation.y = result.position.y + 0.01
			movement_vector.y = 0
	result.clear()
	
	if movement_vector.y <= 0:
		result = space_state.intersect_ray(
			delta_pos(),
			delta_pos() + Vector3.DOWN * 0.75,
			[], 1, true, false
			)
		
		if !result.empty():
			grounded = true
			var angle_to:float = result.normal.angle_to(Vector3.UP)
			if angle_to <= TAU/8:
				translation.y = result.position.y + 0.01
				movement_vector.y = 0
	

func _check_ceilings()->void:
	var result:Dictionary = space_state.intersect_ray(
		delta_pos(),
		delta_pos() + Vector3.UP * hurtbox.height,
		[], 1, true, false
		)
	
	if !result.empty():
		
		var angle_to:float = result.normal.angle_to(Vector3.UP)
		if angle_to > (TAU / 4) + 0.03:
			movement_vector.x = 0
			movement_vector.z = 0
			if movement_vector.y > 0:
				movement_vector.y = 0

func _push_back()->void:
	var collisions:Array = get_overlapping_areas ( )
	for i in collisions.size():
		if collisions[i].get("hurtbox") == null:
			break
		
		var col_translation:Vector3 = collisions[i].translation
		col_translation.y = translation.y
		var direction_to:Vector3 = translation.direction_to(col_translation)
		var distance_to:float = translation.distance_to(col_translation)
		
		if !direction_to:
			var unit_vec:float = rand_range(0.0, TAU)
			direction_to += Vector3(sin(unit_vec), 0, cos(unit_vec))
		var val:float = (1 / (hurtbox.radius + collisions[i].hurtbox.radius)) * distance_to
		val *= val
		movement_vector += -direction_to * (1-(val)) * 2
		
		if i >= 32:
			break

func delta_pos()->Vector3:
	return translation + (movement_vector / Engine.iterations_per_second)
