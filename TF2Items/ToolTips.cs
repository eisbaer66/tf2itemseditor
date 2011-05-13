﻿using System.Windows.Forms;
using TF2Items;

namespace TF2Items
{
	public class ToolTips
	{
		public static string[,] MArrItemToolTips = {	   
										{"damage penalty", "Decreases the weapon's damage by a percentage. Doesn't work on Sniper Rifles."},
										{"damage bonus", "Increases the weapon's damage by a percentage. Doesn't work on Sniper Rifles."},
										{"clip size penalty", "Decreases the cap size of the clip (which carries ammunition to fire with) by a percentage. Only applies to guns."},
										{"clip size bonus", "Increases the cap size of the clip (which carries ammunition to fire with) by a percentage. Only applies to guns. Currently an unused ability."},
										{"fire rate penalty", "Decreases the attack speed by a percentage."},
										{"fire rate bonus", "Increases the attack speed by a percentage."},
										{"heal rate penalty", "Decreases the healing speed by a percentage. Only applies to Mediguns. Currently an unused ability."},
										{"heal rate bonus", "Increases the healing speed by a percentage. Only applies to Mediguns. Currently an unused ability."},
										{"ubercharge rate penalty", "Decreases the Ubercharge build-up speed. Only applies to Mediguns. Currently an unused ability."},
										{"ubercharge rate bonus", "Increases the Ubercharge build-up speed."},
										{"overheal bonus", "Increases the maximum overheal by a percentage. Only applies to Mediguns. Currently an unused ability."},
										{"overheal decay penalty", "Decreases the overheal time by a percentage. Only applies to Mediguns. Currently an unused ability."},
										{"overheal decay bonus", "Increases the overheal time by a percentage. Only applies to Mediguns. Currently an unused ability."},
										{"overheal decay disabled", "Causes the overheal not to decay. Only applies to Mediguns. Currently an unused ability, and doesn't work."},
										{"crit mod disabled", "Disables random critical hits. Only special critical hits work."},
										{"heal on hit for rapidfire", "Each hit on an opponent gives you health by units (i.e. 50 value means 50 health per hit)."},
										{"add uber charge on hit", "Each hit on an opponent gives you Ubercharge by percentage. Only applies to Medics."},
										{"medigun charge is crit boost", "Causes the Ubercharge to grant 100% crits instead of invurenability. Only applies to Mediguns."},
										{"tmp dmgbuff on hit", "Hitting an opponent will grant you a damage buff by a percentage. Lasts for ?? seconds. Currently an unused ability."},
										{"crit vs burning players", "Hitting a burning opponent will always crit."},
										{"dmg penalty vs nonburning", "Hitting a nonburning opponent will deal less damage by a percentage."},
										{"no crit vs nonburning", "Hitting a nonburning opponent will never crit."},
										{"mod flamethrower push", "Disables compression blast. Only applies to Flamethrowers."},
										{"mod flamethrower back crit", "Attacking an opponent from behind (rougly 90 degrees wide behind where the opponent is facing) will always crit. Only applies to Flamethrowers."},
										{"hidden secondary max ammo penalty", "Decreases the secondary max ammo (the backup ammunition to fill your clip), but doesn't tell you in the game. Despite what it says on the description string, it's actually only used by the Flare Gun."},
										{"max health additive bonus", "Increases the player's max health by units."},
										{"alt-fire disabled", "What it name says, it disables the Alt-Fire button. Currently an unused ability, and doesn't work. However, there is a developer weapon called the Rare Bonesaw, which has this ability, but the weapon is not in the file."},
										{"crit mod disabled hidden", "Disables random critical hits, but doesn't tell you in the game. Only special critical hits work."},
										{"alt-fire is vampire", "When I saw the beta Natascha, I knew what this does. When using alt-fire, it functions the same as the regular fire button. However, here's the catch: It heals you on hit, but it does -25% damage in the process. I don't know if this still works or not, so to be tested. Currently a scrapped ability."},
										{"fists have radial buff", "On kill, the player and nearby teammates will gain 50 health and +10% crit chance for a limited time. But in actually using it, now, it doesn't work at all. Only applies to Fists. Currently a scrapped ability."},
										{"critboost on kill", "Killing an opponent will grant the player 100% critical hits for a certain amount of time. Each unit value is a second."},
										{"slow enemy on hit", "Hitting an opponet will cause him to slow down for a second on a percentage chance. Best used on rapidfire weapons."},
										{"set cloak is feign death", "Causes the cloak only to activate by a nonlethal hit while out. Can't attack while holding it out or cloaking. Only applies to Cloaks."},
										{"mult cloak meter consume rate", "Increases the consume rate of the cloak. Only applies to Cloaks."},
										{"mult cloak meter regen rate", "Increases the regneration rate of the cloak while not using the cloak, or when standing still with the Cloak & Dagger, by a percentage. Only applies to Cloaks."},
										{"spread penalty", "Increases the spread by a percentage. Only applies to weapons with 0.01 spread or more."},
										{"hidden primary max ammo bonus", "Decreases the secondary max ammo (the backup ammunition to fill your clip) by a percentage, but doesn't tell you in the game. I did find out why it was called Primary Max Ammo. That means it increases the secondary ammo for the primary weapon."},
										{"mod bat launches balls", "Allows the use of stun balls. Only applies to the Sandman, but do NOT remove the ability from the Sandman, or the game will glitch up."},
										{"dmg penalty vs nonstunned", "Hitting a nonstunned opponent will deal less damage by a certain percentage. This ability was scrapped from the Sandman."},
										{"zoom speed mod disabled", "Disables the slowdown for when you zoom in. Only applies to Sniper Rifles. But right now, it doesn't work. Currently an unused ability."},
										{"sniper charge per sec", "Reduces the charge rate of the Sniper Rifle per second by a percentage. Only applies to, well, Sniper Rifles. Currently an unused ability."},
										{"sniper no headshots", "Disables the ability to headshot. Applies to Sniper Rifles and Huntsmen, but unconfirmed for a reflected arrow."},
										{"scattergun no reload single", "Causes the scattergun to reload two bullets at once. This only works with the Force-a-Nature, but what it ACTUALLY does is reload the entire clip in one go."},
										{"scattergun has knockback", "Greatly increases the knockback of the Scattergun, to the point where you can even use it as a third jump!"},
										{"bullets per shot bonus", "Or pellets, if you want to call them that. Increases the amount of pellets per shot by a percentage. Only applies to guns with pellets."},
										{"sniper zoom penalty", "Decreases the zoom from the Sniper Rifle. Guess what this only applies to? Currently an unused ability."},
										{"sniper no charge", "Disables charging, which means you can't do powerful shots (unless it's a headshot). Currently an unused ability."},
										{"set cloak is movement based", "Sets the Cloak to be movement-based. Standing still will recharge it, and moving will drain it. Used on the Cloak & Dagger. Only applies to Cloaks."},
										{"no double jump", "Disables double jumping. Only applies to Scouts. This was a scrapped ability from the Sandman."},
										{"absorb damage while cloaked", "While cloaked, the cloak will absorb 90% of the damage done to you. Even though on attribute_class it says it wasn't implemented, the Dead Ringer does this, but it doesn't even show it has it on the script. Apparently currently as an unused ability."},
										{"revolver use hit locations", "Enables headshots for the Revolver."},
										{"backstab shield", "Prevents one backstab, and can only be regenerated by the Resupply Cabinet. Unconfirmed if this only works on the Razorback."},
										{"fire retardant", "Prevents afterburn. Doesn't work anymore, but it was going to be used on the Fire-proof Suit. Sadly, it was scrapped."},
										{"move speed penalty", "Decreases your movement speed by a percentage."},
										{"obsolete ammo penalty", "Unknown. Does nothing. Currently an unused ability."},
										{"jarate description", "Simply tells you what the Jarate does."},
										{"health regen", "Regenerates health (by units) per second."},
										{"self dmg push force increased", "Increases the knockback (by a percentage) from rocket-/sticky-jumps. Currently an unused ability."},
										{"self dmg push force decreased", "Decreases the knockback (by a percentage) from rocket-/sticky-jumps. Currently an unused ability."},
										{"dmg taken from fire reduced", "Reduces the damage you take from fire on a percentage."},
										{"dmg taken from fire increased", "Increases the damage you take from fire on a percentage."},
										{"dmg taken from crit reduced", "Reduces the damage you take from critical hits on a percentage. Unconfirmed if having 100% critical-proof will prevent backstabs. If so, I can see why this is a currently unused ability."},
										{"dmg taken from crit increased", "Increases the damage you take from critical hits on a percentage. Beware the critical Pistols. Currently an unused ability."},
										{"dmg taken from blast reduced", "Reduces the damage you take from explosions on a percentage."},
										{"dmg taken from blast increased", "Increases the damage you take from explosions on a percentage."},
										{"dmg taken from bullets reduced", "Reduces the damage you take from bullets on a percentage. Currently an unused ability to prevent the possibility of outsmarting boolets."},
										{"dmg taken from bullets increased", "Increases the damage you take from bullets on a percentage."},
										{"increase player capture value", "Increases the capture value on Control Points and Carts by units."},
										{"health from healers reduced", "Decreases the amount of health you get from Medics by a percentage. Currently an unused ability."},
										{"health from healers increased", "Increases the amount of health you get from Medics by a percentage. Currently an unused ability."},
										{"weapon burn dmg increased", "Increases the damage that your fire does by a percentage. Given that only Pyro has fire damage, I'll say that it only applies to Pyros. Currently an unused ability."},
										{"weapon burn dmg reduced", "Decreases the damage that your fire does by a percentage. Technically only applies to Pyros. Currently an unused ability."},
										{"weapon burn time increased", "Increases the afterburn time on the opponents you inflicted on by a percentage. Technically only applies to Pyros. Currently an unused ability."},
										{"weapon burn time reduced", "Decreases the afterburn time on the opponents you inflicted on by a percentage. Technically only applies to Pyros. Currently an unused ability."},
										{"aiming movespeed increased", "Decreases the slowdown speed on certain aiming points, such as when scoping with a Sniper Rifle or spinning the Minigun. Currently an unused ability."},
										{"maxammo primary increased", "Increases the primary's secondary ammunition by a percentage. Currently an unused ability."},
										{"maxammo primary reduced", "Decreases the primary's secondary ammunition by a percentage. Currently an unused ability."},
										{"maxammo secondary increased", "Increases the secondary ammunition by a percentage."},
										{"maxammo secondary reduced", "Decreases the secondary ammunition by a percentage. Currently an unused ability."},
										{"maxammo metal increased", "Increases the maximum metal that you can hold onto by a percentage. Can only go up to 999 before it restarts (therefore, the maximum value should be 4.99) by a percentage. Unused."},
										{"maxammo metal reduced", "Decreases the maximum metal that you can hold onto by a percentage. Unused."},
										{"cloak consume rate increased", "Increases the cloak consume rate by a percentage. Only applies to Cloaks. I thought this was already made?"},
										{"cloak consume rate decreased", "Decreases the cloak consume rate by a percentage. Only applies to Cloaks."},
										{"cloak regen rate increased", "Increases the cloak regeneration rate by a percentage. Only applies to Cloaks."},
										{"cloak regen rate decreased", "Decreases the cloak regeneration rate by a percentage. Only applies to Cloaks."},
										{"minigun spinup time increased", "Increases the spin-up time for the Minigun by a percentage. Only applies to Miniguns."},
										{"minigun spinup time decreased", "Decreases the spin-up time for the Minigun by a percentage. Only applies to Miniguns. Unused."},
										{"max pipebombs increased", "Increases the amount of Sticky Bombs you can have out. Only applies to Stickybomb Launchers."},
										{"max pipebombs decreased", "Decreases the amount of Sticky Bombs you can have out. Only applies to Stickybomb Launchers. Unused."},
										{"SRifle Charge rate increased", "Increases the Sniper Rifle charge rate. Only applies to Sniper Rifles. Currently an unused ability."},
										{"SRifle Charge rate decreased", "Decreases the Sniper Rifle charge rate. Only applies to Sniper Rifles. Unused."},
										{"Construction rate increased", "Increases the construction rate of the Wrench by percentage. Only applies to Wrenches."},
										{"Construction rate decreased", "Decreases the construction rate of the Wrench by percentage. Only applies to Wrenches. Unused."},
										{"Repair rate increased", "Increases the repair rate of the Wrench by percentage. Only applies to Wrenches. Unused."},
										{"Repair rate decreased", "Decreases the repair rate of the Wrench by percentage. Only applies to Wrenches. Unused."},
										{"Reload time increased", "Increases the reload time. Only applies to guns. Unused."},
										{"Reload time decreased", "Decreases the reload time. Only applies to guns. Unused."},
										{"selfdmg on hit for rapidfire", "Hitting an opponent will get you hurt (by units). Unused, and probably for a good reason."},
										{"Blast radius increased", "Increases the radius of explosions by a percentage. Only applies to explosions. Unused."},
										{"Blast radius decreased", "Decreases the radius of explosions by a percentage. Only applies to explosions."},
										{"Projectile range increased", "Increases the range of projectiles by a percentage. Although, why? Unused, and for a good reason."},
										{"Projectile range decreased", "Decreases the range of projectiles by a percentage. Unused."},
										{"Projectile speed increased", "Increases the projectile speed by a percentage. Only seems to apply to the rockets."},
										{"Projectile speed decreased", "Decreases the projectile speed by a percentage. Only seems to apply to the rockets. Unused."},
										{"overheal penalty", "Decreases the max overheal by a percentage. Only applies to Mediguns. Unused."},
										{"weapon spread bonus", "Reduces the spread by a percentage. Only applies to guns. Unused."},
										{"move speed bonus", "Increases your speed by a percentage. Currently an unused ability. The max is the Scout, but if you have 500% on the Heavy, he will have no slowdown while spinning his Minigun! The following chart is the minimum necessary percentage to be as fast as the Scout:"},
										{"health from packs increased", "Increases the amount of health you get from health packs by a percentage. Unused."},
										{"health from packs decreased", "Decreases the amount of health you get from health packs by a percentage. Unused."},
										{"heal on hit for slowfire", "See 'heal on hit for rapidfire', except this is unused."},
										{"selfdmg on hit for slowfire", "See 'selfdmg on hit for slowfire'."},
										{"ammo regen", "Every 5 seconds, a percent of your ammunition gets recovered (i.e. if the value is set to 0.1, you recover 20 ammo every 5 seconds with the Minigun). Unused"},
										{"metal regen", "See ammo regen, but instead of ammo, it's Metal. Unused."},
										{"mod mini-crit airborne", "Any rocket that hits an airborne enemy pushed back by your rocket will cause a mini-crit. Only applies to the Direct Hit."},
										{"mod shovel damage boost", "The further you take damage, the more damage you can do and move faster, but only when active. Only applies to the Equalizer and Soldier."},
										{"mod soldier buff type", "Buff Banner effect."},
										{"dmg falloff increased", "Increases the damage falloff by a percentage, which means the longer the projectile goes, the less damage. Only applies to rockets. Unused, and doesn't work."},
										{"dmg falloff decreased", "Decreases the damage falloff by a percentage. Only applies to rockets. Unused, and doesn't work."},
										{"sticky detonate mode", "Makes it that you activate the stickies near or on your crosshair. Currently a used attribute."},
										{"sticky arm time penalty", "Increases the sticky arm time by units."},
										{"stickies detonate stickies", "Makes the stickies able to destroy the enemies' stickies."},
										{"mod demo buff type", "It does ABSOLUTELY NOTHING. Currently an unused attribute."},
										{"speed boost when active", "Increases your speed by a percentage, only when active. Currently an unused attribute."},
										{"mod wrench builds minisentry", "Makes the Wrench build Mini-Sentries instead of large Sentries."},
										{"max health additive penalty", "Decreases your max health by a percentage. Currently an unused attribute."},
										{"sticky arm time bonus", "Reduces the sticky arm time by units. Currently an unused attribute."},
										{"sticky air burst mode", "The stickybombs burst when it touches ANYTHING. Doesn't work if set to 1. Setting this to 2 makes grenade launcher grenades disappear when it hits something other than an enemy."},
										{"provide on active", "Makes it so that ALL the attributes the weapon provides only be used when the said weapon is active."},
										{"health drain", "For each second, you lose health (based on the value, which is in units)."},
										{"medic regen bonus", "Increases the Medic's regeneration power by a percentage. Only applies to Civilians...er, Medics. Currently an unused attribute."},
										{"medic regen penalty", "Decreases the Medic's regeneration power by a percentage. Currently an unused attribute."},
										{"community description", "Just adds the Community description."},
										{"soldier model index", "Just adds the description from the Medal, when you were one of the first 1,111 visitors."},
										{"attach particle effect", "Attaches a particle effect. 1 to 3 (heh) are flames, 4 is the Community Sparkle, and 5 is the Cheater's Lament Particle."},
										{"rocket jump damage reduction", "Reduces the damage you take from self damage (not just rocket jumping!) at a percentage."},
										{"mod sentry killed revenge", "When using the weapon with the ability, for each kill the Sentry gets, you get 2 guaranteed crit shots, and for each assist, you get 1 guaranteed crit shot. The crit shots only apply once the Sentry is destroyed. Only applies to the Engineer."},
										{"dmg bonus vs buildings", "The weapon deals more damage to enemy buildables at a percentage."},
										{"dmg penalty vs players", "The weapon deals less damage to opponents at a percentage."},
										{"lunchbox adds maxhealth bonus", "Gives you an additional 50 HP for 30 seconds. Only applies to the Dalokohs Bar."},
										{"hidden maxhealth non buffed", "It just boosts your max health, but without telling you so."},
										{"selfmade description", "Just adds the Selfmade description. No, you can't edit it, it's not what you think it is."},
										{"set item tint RGB", "Sets the RGB on tinted items"},
										{"custom employee number", "The description of the Bronze, Silver, Gold, and Platinum badges. It is based on when you joined the game. "},
										{"lunchbox adds minicrits", "Not only allows you to do Mini-Crit damage, but sets various other options on the Natascha, Gloves of Running Urgently, Buffalo Steak Sandvich and Your Eternal Reward"},
										{"damage applies to sappers", "Gives the weapon the ability to be able to remove sappers based on your damage."},
										{"Wrench index", "The number of the Golden Wrench obtained. Only applies to Golden Wrenches"},
										{"building cost reduction", "Reduces the cost of buildings"},
										{"bleeding duration", "Makes the opponent bleed on hit. The duration is based on the value, in seconds."},
										{"turn to gold", "Turns any victim that is killed by the weapon turn into a gold statue. Only applies to the Wrenches."},
										{"cannot trade", "Weapon can not be traded"},
										{"disguise on backstab", "Instantly disguises as the backstab victim. Only applies to Spies."},
										{"cannot disguise", "Wearer can't disguise. Only applies to Spies"},
										{"silent killer", "Almost instantly removes the victim's ragdoll."},
										{"disguise speed penalty", "Speed is reduced while disguised. Only applies to Spies."},
										{"add cloak on kill", "Adds cloak when you kill someone. Only applies to Spies."},
										{"cloak blink time penalty", "Half-visible time during a cloak is reduced. Only applies to Spies."},
										{"quiet unstealth", "Decloak sound is reduced. Only applies to Spies."},
										{"flame size penalty", "Flamethrower flame size is reduced. Only applies to Pyros."},
										{"flame size bonus", "Flamethrower flame size is increased. Only applies to Pyros."},
										{"flame life penalty", "Afterburn lasts a shorter amount of time. Only applies to Pyros."},
										{"flame life bonus", "Afterburn lasts longer. Only applies to Pyros."},
										{"charged airblast", "Airblast is charged. Only applies to Pyros."},
										{"add cloak on hit", "Increases cloak on hit. Only applies to Spies."},
										{"disguise damage reduction", "Damage is reduced while disguised. Only applies to Spies."},
										{"disguise no burn", "Unable to burn while disguised. Only applies to Spies."},
										{"dmg from sentry reduced", "Damage taken from sentries is reduced."},
										{"airblast cost increased", "Airblasting takes more ammo. Only applies to Pyros."},
										{"airblast cost decreased", "Airblasting takes less ammo. Only applies to Pyros."},
										{"purchased", "Probably the games puts this attribute on purchased items."},
										{"flame ammopersec increased", "Firing the flamethrower takes more ammo per second. Only applies to Pyros."},
										{"flame ammopersec decreased", "Firing the flamethrower takes less ammo per second. Only applies to Pyros."},
										{"jarate duration", "Applies Jarate effects to target on hit."},
										{"no death from headshots", "Weapon can't headshot. Only applies to Snipers."},
										{"deploy time increased", "Switching weapons is slower."},
										{"deploy time decreased", "Switching weapons is faster"},
										{"minicrits become crits", "Every time he weapon should mini-crit it crits instead."},
										{"heal on kill", "Adds health on kill."},
										{"no self blast dmg", "Own explosive weapons can't damage user. Only applies to Demomen and Soldiers."},
										{"slow enemy on hit major", "Slows enemies on hit."},
										{"aiming movespeed decreased", "Makes you move slower while spinning your Minigun or preparing a shot from your Sniper Rifle/Huntsman. (i.e. if value is 0, you can't move while doing one of the two) Only applies to Miniguns, Sniper Rifles, and Huntsmen."},
										{"duel loser account id", "Adds the duel loser ID to duel badge."},
										{"event date", "Adds last duel date to duel badge."},
										{"gifter account id", "Adds gifter account to item."},
										{"set supply crate series", "Sets supply crate series number."},
										{"preserve ubercharge", "Stores the given percentage of your Ubercharge on death (if you died with 40% charge, it stores 8%)"},
										{"elevate quality", "This does absolutely nothing."},
										{"active health regen", "Heals you while the item is out."},
										{"active health degen", "Drains your health while the item is out."},
										{"referenced item id low", "No idea."},
										{"referenced item id high", "No idea."},
										{"referenced item def", "No idea."},
										{"always tradable", "Makes the item always tradeable, even after purchase."},
										{"noise maker", "The item is a noise maker."},
										{"halloween item", "Tells you it is a Halloween Item of a specific year."},
										{"fires healing bolts", "Makes the bolts heal, based on distance. Only applies to Crossbows."},
										{"enables aoe heal", "When you taunt, you heal everyone that is around you. Only applies to Medic."},
										{"gesture speed increase", "Makes your taunt go faster. In case of taunt kills, the killing point remains the same, it's just the animation that's doubled. This also goes for the stun point, if it does stun."},
										{"charge time increased", "Makes your charge for the Chargin' Targe last longer. Value is in seconds, and is additive. Even if it appears that the charge meter is empty, it will still keep going, and will retain the crit ability, until it runs out."},
										{"drop health pack on kill", "Whenever ANY of your weapons kill, the corpse will drop a small health pack."},
										{"hit self on miss", "If you miss, you hit yourself. Even effects can oppose you, like bleeding. Only applies to Melee weapons."},
										{"dmg from ranged reduced", "While using the weapon, you will take less damage from any non-melee attacks. Also affects taunt kills."},
										{"dmg from melee increased", "While using the weapon, you will take more damage from any melee attacks."},
										{"blast dmg to self increased", "Makes you more vulnerable from self-explosive damage, but only for that specific weapon, unlike being resistant to self-explosive damage."},
										{"Set DamageType Ignite", "Hitting someone will cause them to ignite."},
										{"minicrit vs burning player", "Hitting burning enemies deals mini-crit damage."},
										{"sanguisuge", "Backstabing someone gives you your victim's remaining health. This will overheal you up to 3x to your normal health. Only applies to knives."},
										{"mark for death", "Anyone you hit will be marked with a skull above their head for 15 seconds. Marked players take mini-crit damage."},
										{"decapitate type", "Yet to be tested. Presumably either adds blood to your Half-Zaitoichi or insta-kills anyone wielding the same weapon as you."},
										{"restore health on kill", "Restores a percentage of your health."},
										{"honorbound", "The weapon cannot be put away until it kills."},
										{"paint effect", "Unknown. Unused."},
										{"tradeable after date", "Makes it so it is only tradable after this specified date has passed."},
										{"force level display", "Forces the item to have a specified cosmetic level."},
										{"random level curve replacement", "Makes the item have a random level curve on drop."},
										{"apply z velocity on damage", "Any hit will make the enemy go upward. Setting this to a negative number will make them fall down."},
										{"apply look velocity on damage", "Any hit will make the enemy go towards the direction you were looking at. Setting this negative will pull them towards you."},
										{"mult decloak rate", "Not really sure, to be tested."},
										{"mult sniper charge after bodyshot", "Sets the sniper rifle charge speed after doing a bodyshot."},
										{"mult sniper charge after miss", "Sets the sniper rifle charge speed after missing a shot."},
										{"dmg bonus while half dead", "Damage boost when your health is below half."},
										{"dmg penalty while half alive", "Damage penalty when your health equal to or higher than half."},
										{"makers mark id", "Probably marks your item with an ID."},
										{"unique craft index", "Shows what number your crafted item is."},
										{"mod medic healed damage bonus", "Makes your weapon make more damage when being healed by a Medic or a Dispener."},
										{"mod medic healed deploy time penalty", "Makes your have slower weapon switch time when not being healed by a Medic or a Dispenser."},
										{"mult sniper charge after headshot", "Sets the sniper rifle charge speed after getting a headshot."},
										{"mod medic killed revenge", "When the Medic healing you dies, you get this many seconds of crits."},
										{"medigun charge is megaheal", "Sets the medigun charge to be megaheal."},
										{"mod medic killed minicrit boost", "When the Medic healing you dies, you get this many seconds of mini-crits."},
										{"mod shovel speed boost", "The less you have health, the faster you'll move. Only applies to the Equalizer."},
										{"mod weapon blocks healing", "Blocks healing when the weapon is out."},
										{"minigun no spin sound", "Minigun's don't make a sound."},
										{"ubercharge rate bonus for healer", "Having this weapon out will make the Medic healing you get Ubercharge faster."},
										{"reload time decreased while healed", "When being healed, you will reload faster."}
									 };
		public static string FindToolTip(string str)
		{
			for (int i = 0; i < MArrItemToolTips.Length / 2; i++)
			{
				if (MArrItemToolTips[i, 0] == str) return MArrItemToolTips[i, 1];
			}
			return "Unknown";
		}

	}
}
