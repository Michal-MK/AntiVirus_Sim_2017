﻿using System.Collections.Generic;

namespace Igor {
	namespace Constants {
		namespace Strings {
			public class SceneNames {
				public const string MENU_SCENE = "MainMenu";
				public const string GAME1_SCENE = "GameScene";
				public const string LEADERBOARD_SCENE = "HighScores";
				public const string SAVES_SCENE = "SaveHistory";
				public const string HELP1_SCENE = "Help1";
				public const string HELP2_SCENE = "Help2";
				public const string CREDITS_SCENE = "Credits";
			}

			public class ObjNames {
				public const string BULLET_PICKUP = "FiredBullet";
				public const string BULLET = "Bullet";
				public const string BOMB = "Bomb";
				public const string BOMB_PICKUP = "BombPickup";
				public const string COIN = "Coin";
				public const string SPIKE = "Spike";
				public const string MENU_CHOOSE_DIFFICULTY = "Select Difficulty";
				public const string AVOIDANCE_SIGN = "_SignPost Avoidance";
			}

			public class BackgroundNames {
				public const string BACKGROUND_1 = "Background_1";
				public const string BACKGROUND_2 = "Background_2";
				public const string BACKGROUND_3 = "Background_3";
				public const string BACKGROUND_4 = "Background_4";
				public const string BACKGROUND_MAZE = "Background1_Maze";
				public const string BACKGROUND_BOSS_X = "Background_Boss_";
			}

			public class Boss {
				public const string BOSS_HEALTH_PLACEHOLDER = "_BossHealthPlaceHolder";
			}

			public class PrefabNames {
				public const string ENEMY_PROJECTILE_ACCURATE = "Emeies/EnemyProjectile_Accurate";
				public const string ENEMY_PROJECTILE_INACCUARATE = "Enemies/EnemyProjectile_Inaccurate";
				public const string ENEMY_PROJECTILE_ICICLE = "Enemies/EnemyProjectile_Icicle";
				public const string ENEMY_KILLERBLOCK_BOSS = "Enemies/Boss/Enemy_KillerBlockBoss";
				public const string ENEMY_KILLERBLOCK = "Enemies/KillerBlock";
				public const string ENEMY_TURRET = "Enemies/Turret";
			}

			public class EnemyNames {
				public const string ENEMY_TURRET = "TurretAttack";
			}

			public class InputNames {
				public const string MOUSEWHEEL = "Mouse Scroll Wheel";
				public const string SUBMIT = "Submit";
				public const string MOVEMENT_HORIZONTAL = "HorMovement";
				public const string MOVEMENT_VERTICAL = "VertMovement";
			}

			public class Layers {
				public const string WALLS = "Walls";
				public const string PLAYER = "Player";
				public const string PLAYER_COLLISIONS = "PlayerCollisions";
				public const string BACKGROUNDS = "Background";
				public const string BG_FEATURES = "BackgroundFeatures";
				public const string DARK_ENEMIES = "DarkModeEnemies";
				public const string LIGHT_ENEMIES = "LightModeEnemies";
			}

			public class Tags {
				public const string UNTAGGED = "Untagged";
				public const string ENEMY = "Enemy";
				public const string ENEMY_INACTIVE = "EnemyInactive";
				public const string BACKGROUND = "BG";
			}

			public class Settings {
				public const string MUSIC_VOL = "music_audio";
				public const string FX_VOL = "effect_audio";
			}
		}

		namespace Integers {
			public class Layers {
				public enum Layer {
					DEFAULT,
					UI = 5,
					WALL = 8,
					PLAYER,
					PLAYER_COLLISIONS,
					BACKGROUND,
					BACKGROUND_ELEMENTS,
					DARK_ENEMIES,
					LIGHT_ENEMIES,
					DAMAGE_COLLISION
				}
			}
		}
	}
}
