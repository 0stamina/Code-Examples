extends Actor
class_name ALifeCreature

export(Resource) var data
onready var mesh_instance:MeshInstance = $misp/metarig/Skeleton/misp
onready var animation_player:AnimationPlayer = $misp/AnimationPlayer
onready var global_pathfinding:Navigation = null
var target_pos:Vector3
var path:PoolVector3Array

var toggle:bool = true

var navmesh_height:float

#factors of 840
#[1, 2, 3, 4, 5, 6, 7, 8, 10, 12, 14, 15, 20, 21, 24, 28, 30, 35,
# 40, 42, 56, 60, 70, 84, 105, 120, 140, 168, 210, 280, 420, 840]
var physics_frame:int

func _ready():
	get_parent().get_node("DebugInfo/Panel/RichTextLabel").misps += 1
	var node:Node = get_tree().current_scene.find_node("Navigation")
	
	mesh_instance.mesh.surface_set_material(0, SpatialMaterial.new())
	var unit_vec:float = rand_range(0.0, TAU)
	data.personality_c_p = sin(unit_vec) * randf()
	data.personality_s_m = cos(unit_vec) * randf()
	
	mesh_instance.get_surface_material(0).set_shader_param("personality_c_p", data.personality_c_p)
	mesh_instance.get_surface_material(0).set_shader_param("personality_s_m", data.personality_s_m)
	movement_speed = rand_range(2.0, 2.0)
	
	if node:
		assert(node.is_class("Navigation"))
		global_pathfinding = node as Navigation
		var navmesh:NavigationMesh = global_pathfinding.get_node("NavigationMeshInstance").navmesh
		navmesh_height = navmesh.get("cell/height") * 2
	
	

func _physics_process(_delta):
	physics_frame += 1
	if translation.y < -10:
		get_parent().get_node("DebugInfo/Panel/RichTextLabel").misps -= 1
		queue_free()
	space_state = get_world().direct_space_state
	
	if global_pathfinding:
		randomize()
		
		find_path(Vector3(data.personality_c_p * 10.0, 0, data.personality_s_m * 10.0) + Vector3.RIGHT * 15)
		
		var check_pos:Vector3 = Vector3(target_pos.x, translation.y, target_pos.z)
		var distance_to:float = translation.distance_to(check_pos)
		
		if !path.empty() and distance_to > (hurtbox.radius):
			var index = 1
			var path_pos:Vector3 = Vector3(path[1].x, translation.y, path[1].z)
			var first_index_distance_to:float = translation.distance_to(path_pos)
			
			while translation.distance_to(path_pos) <= (hurtbox.radius) and index < path.size() - 1:
				index += 1
				path_pos = Vector3(path[index].x, translation.y, path[index].z)
			
			var direction_to:Vector3 = translation.direction_to(path[index])
			direction_to.y = 0
			direction_to = direction_to.normalized()
			
			mesh_instance.rotation.y = lerp_angle(mesh_instance.rotation.y, atan2(direction_to.x, direction_to.z), 10.0 / Engine.iterations_per_second)
			movement_vector = direction_to * movement_speed
			
			if is_zero_approx(physics_frame % 120 * log(distance_to + 1)) || (first_index_distance_to <= (hurtbox.radius)):
				path = global_pathfinding.get_simple_path(translation, target_pos)
		
		
		if distance_to <= (navmesh_height + hurtbox.radius):
			path.resize(0)
	
	
	if grounded and Vector2(movement_vector.x, movement_vector.z).length() > 0:
		animation_player.play("walk", -1, Vector2(movement_vector.x, movement_vector.z).length() * 1.5)
	elif animation_player.current_animation != "idle":
		animation_player.play("idle")
	
	movement_vector.y = -5
	
	_push_back()
	
	_check_walls()
	_check_floors()
	_check_ceilings()
	
	translation = delta_pos()
	
	movement_vector = Vector3()
	
	physics_frame = physics_frame % 840

func find_path(target:Vector3):
	
	if grounded and path.empty():
		target_pos = Vector3(target.x, target.y + min_wall_height, target.z)
		target_pos = global_pathfinding.get_closest_point(target_pos)
		path = global_pathfinding.get_simple_path(translation, target_pos)
		toggle = false
	
