extends AnimatedSprite2D

signal act
signal doneTransitioning
var emitted = false
var sfxPlayed = false
var sfx = load("res://assets/sfx/transition.ogg")

func _process(_delta: float) -> void:
	if Globals.transitioning:
		if !sfxPlayed:
			$"/root/Audio/SFX".volume_db = 2
			$"/root/Audio/SFX".stream = sfx
			$"/root/Audio/SFX".play()
			$"/root/Audio/SFX".volume_db = 0
			sfxPlayed = true
		if frame == 0 and !is_playing():
			play("default")
		if frame == 7 and !emitted:
			emitted = true
			act.emit()
			$"../Title".hide()
			$"../Game".show()
		if frame == 14 and !is_playing():
			Globals.transitioning = false
			emitted = false
			sfxPlayed = false
			doneTransitioning.emit()
			frame = 0
