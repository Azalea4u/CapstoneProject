EXTERNAL StartGame()
EXTERNAL QuitGame()
-> main

=== main ===
Are you ready to start?
    + [Start Game]
        -> start_game
    + [Go Back]
        -> go_back
    + [Quit Game]
        -> quit_game
        
=== start_game ===
Don't die!
~StartGame()
-> final_end

=== go_back ===
What do you even need to do???
-> final_end

=== quit_game ===
~QuitGame()
-> final_end

=== final_end ===
-> END