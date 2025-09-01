# Documentation - Ajouter d'un nouveau niveau

## Objectif
Cette documentation explique les différentes étapes pour ajouter une nouveau niveau jouable à notre jeu. Elle permet
à n'importe quelle personne qui reprend le projet, même sans connaissance du code, de créer un niveau avec ses textures.

## Étapes

### 1. Menus et palette
1. Activer les différents menus qui nous seront utiles par la suite. Les deux menus se trouvent au même endroit : **Tools -> ProBuilder -> Editors -> Open Material Editor ET Open UV Editor**.
- Material Editor -> permet de créer des palettes pour ajouter les textures à nos formes.
- UV Editor -> permet de modifier les différents paramètres du matériau. Les 2 paramètres utilisés la plupart du temps sont la **Rotation** qui permet de tourner le motif et le **Tiling** qui permet de modifier le nombre de fois que le motif doit se répéter sur la forme. Vu que les formes sont assez grandes, on utilise principalement le **16** sur le tiling.
2. Ajouter de nouveau assets que l'on trouve dans le **unity store** ou utiliser les assets existants dans **Assets -> ProceduralGeneration -> Materials**.
3. Dans le nouvel onglet **Material Editor** ajouter une nouvelle palette avec le bouton **New Material Palette**. Une nouvelle palette sera créée à la base du dossier **Assets**. Renommer ce fichier pour le trouver par la suite.
4. Trouver les asssets que l'on veut utiliser et les glisser dans notre nouvelle palette afin de pourvoir les utiliser simplement.

### 2. Dupliquer les pièces
1. Se rendre dans le dossier ou se trouvent les différentes pièces : **Assets -> ProceduralGeneration -> Rooms -> 64x64**.
2. Dupliquer le dossier **PrototypeRooms**. Raccourcis: sélectionner le dossier et appuier sur **ctrl + d**.
3. Renommer le nouveau dossier comme le reste, **Level_x_Rooms**. Ce dossier contient toutes les dispositions des pièces existantes.

### 3. Texturer les pièces
1. Double cliquer sur une des 21 pièces existantes.
2. Sélectionner le bon outil afin de pouvoir sélectionner les différents éléments de la pièce. Il faut choisir les outils comme sur l'image suivante, sinon ça ne va pas fonctionner.
![tool to select](./img/select_form.png)
3. Cliquer sur un mur par exemple, aller dans la palette et choisir la texture que l'on veut appliquer.
4. Se rendre dans l'onglet **UV Editor** et choisir le bon **Tiling**.
5. Répéter ces étapes pour tous les éléments de la pièces.
6. Répéter ces étapes pour toutes les pièces existantes.
