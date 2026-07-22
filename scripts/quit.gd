extends TextureButton

func _pressed() -> void:
	$"/root/Audio/SFX".play()
	get_tree().quit()
