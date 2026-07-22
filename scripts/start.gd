extends TextureButton


func _pressed() -> void:
	$"/root/Audio/Button".play()
	Globals.transitioning = true
#	Globals.playing = true
