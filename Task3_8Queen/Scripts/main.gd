extends Node2D

const width = 8
const height = 8
var count = 0
var enable = true
var cells = []
var buttons = []

# Called when the node enters the scene tree for the first time.
func _ready():
	$Label.hide()
	for y in range(height):
		cells.append([])
		cells[y] = []
		buttons.append([])
		buttons[y] = []
		for x in range(width):
			cells[y].append([])
			cells[y][x] = 0
			buttons[y].append([])
			buttons[y][x] = str("Button" , y , "," , x)


func check_cells():
	var dir = [
		Vector2(-1, -1), Vector2(0, -1), Vector2(1, -1),
		Vector2(-1,  0),                 Vector2(1,  0),
		Vector2(-1,  1), Vector2(0,  1), Vector2(1,  1)]
	for y in range(height):
		for x in range(width):
			if cells[y][x] == 0:
				continue
			for i in dir:
				for j in range(1, 8):
					if y+(i.y*j) < 0 || height <= y+(i.y*j) \
				 	|| x+(i.x*j) < 0 || width <= x+(i.x*j):
						continue
					if cells[y+(i.y*j)][x+(i.x*j)] == 1:
						$AudioGameOver.play()
						return "Game Over!"
	$AudioGameClear.play()
	return "Game Clear!"


func _on_Button_pressed(extra_arg_0):
	if enable:
		if cells[extra_arg_0.y][extra_arg_0.x] == 0:
			get_node(buttons[extra_arg_0.y][extra_arg_0.x]).text = "Q"
			$AudioButtonPush1.play()
			cells[extra_arg_0.y][extra_arg_0.x] = 1
			count += 1
		else:
			get_node(buttons[extra_arg_0.y][extra_arg_0.x]).text = ""
			$AudioButtonPush2.play()
			cells[extra_arg_0.y][extra_arg_0.x] = 0
			count -= 1

	if count == 8:
		enable = false
		$Label.text = check_cells()
		$Label.show()
