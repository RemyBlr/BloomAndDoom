You cannot use a constructor for a MonoBehaviour like Projectile.

The reason is that you never create MonoBehaviour instances with new Projectile(). The Unity engine creates them for you when you call Instantiate(prefab) or AddComponent<Projectile>(). Since the engine handles the creation, it uses a parameterless constructor, and you don't have an opportunity to pass in arguments.

Therefore, you must use a public method (like a setter or an initialization method) to configure the projectile's properties after it has been instantiated.

A common and good practice is to create a single public Initialize method that acts like a constructor, setting up all the necessary properties at once.annot use constructors in Unity



AI Perception

Bonnes pratiques de perception (Unity)
Séparer la perception du reste (mouvement/combat/anim) pour garder la FSM propre.
Vision:
FOV: angle et rayon (test angle avec Vector3.Angle).
Ligne de vue: Raycast depuis les “yeux” vers la cible, ignorer triggers, filtrer par LayerMask “Environment”.
Audition:
Définir un rayon d’écoute et une intensité du bruit (Overlaps/Distance) → met à jour la dernière position connue.
Mémoire:
Conserver la dernière position vue/entendue pendant N secondes (memoryDuration) pour “chercher” après perte de vue.
Événements:
Déclencher OnTargetSpotted/OnTargetLost/OnNoiseHeard pour orchestrer les transitions d’état.
Performance:
Cibler les layers (LayerMask), éviter les allocations, time-slicer (ne pas faire toutes les vérifs chaque frame pour tous les agents), gizmos activables seulement en sélection.



## AI Sound Perception

TOUT les objets qui doivent bloquer ou réfléchir le son doivent avoir le composant *Steam Audio Geometry* -> On peut aussi leur assigner des matériaux acoustiques du type brique, verre.