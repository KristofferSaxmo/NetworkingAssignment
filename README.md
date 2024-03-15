# NetworkingAssignment
 
Overall Score: 8

### 1. Overhead Names [Points: 1]

Classes: [Name](https://github.com/KristofferSaxmo/NetworkingAssignment/blob/main/Assets/Scripts/Player/Name.cs), [NameUI](https://github.com/KristofferSaxmo/NetworkingAssignment/blob/main/Assets/Scripts/UI/NameUI.cs)

I use SavedClientInformationManager to get the UserData which in turn holds the player's name.
To get the corresponding name for each player, I check the NetworkObjects OwnerClientID.
This is done by the server since it only needs to be done once, and since it's stored in a NetworkVariable it will be synced for all clients.
In NameUI, I bind the name to a method that updates the UI text with the player's name.

### 2. Health Packs [Points: 1]

Class: [MedKit](https://github.com/KristofferSaxmo/NetworkingAssignment/blob/main/Assets/Scripts/Pickups/Medkit.cs)

Just like the mines, I use a simple trigger to check if the player is standing on the med kit.
When the med kit is used, it creates a new med kit that is spawned in a random position before destroying itself.
The only differences between the med kit and the mine is that the med kit heals the player, and it checks if the player has less than full health before being used.
The only notable networking thing is that the logic is handled by the server only.

### 3. Sprite Renderer [Points: 1]

Class: [PlayerController](https://github.com/KristofferSaxmo/NetworkingAssignment/blob/main/Assets/Scripts/Player/PlayerController.cs)

I added a list of sprites that will be used for the moving animation.
When the player moves, an coroutine is called to run as long as the player keeps on moving.
During that time, the coroutine cycles through the animation sprites with the SpriteRenderer.
When the player stops moving, the coroutine is stopped and the SpriteRenderer is set to the default sprite.
The network logic for this is that that the Owner handles the movement as it should, but the clients handles the animation.
It would either be the server or the client that handles the animation, but it's not code that needs to be secure which led me to use the client.

### 9. Player Death [Points: 1]

Class: [Health](https://github.com/KristofferSaxmo/NetworkingAssignment/blob/main/Assets/Scripts/Player/Health.cs)

This is really simple.
The server already handles the health, so what I added is that when the health reaches <=0, the players NetworkObject is despawned.

### 11. Limited Respawns [Points: 2]

Class: [Health](https://github.com/KristofferSaxmo/NetworkingAssignment/blob/main/Assets/Scripts/Player/Health.cs)

I gave each player 3 lives in the form of a NetworkVariable.
When the player dies, the lives are decreased by 1.
Respawning means setting the player's health to full and moving the player to the start position.
In our case, the server has access to the health, and the owner has access to the transform.
This means that we need to go from a server method to a owner method to set the transform.
I did this by calling a Rpc method from the server to the owner to set the transform, and then going back to the server to set the health.
Once the player reaches 0 lives, it despawns for good.

### 13. Live Scoreboard [Points: 2]

Classes: [ScoreboardUI](https://github.com/KristofferSaxmo/NetworkingAssignment/blob/main/Assets/Scripts/UI/ScoreboardUI.cs), [KillTracker](https://github.com/KristofferSaxmo/NetworkingAssignment/blob/main/Assets/Scripts/Trackers/KillTracker.cs)

All damage sources now have a ClientID reference to the player that dealt the damage. If it's a non-player source, like a mine, the KillTracker wont be updated.
When a player dies, the KillTracker updates the killer's score.
The score is stored in 3 parallel lists, because of limitations with NetworkVariables and NetworkLists.
The lists are the ClientID, the player's name and the player's score.
ScoreboardUI is subscribed to KillTrackers kill list OnListChanged event, and will remake the scoreboard when the someone gets a kill.
The scoreboard is sorted by score, with the highest score at the top and names next to the scores.
KillTracker is server-side and ScoreboardUI is client-side, so the server syncs the data and the client updates their UIs with it.