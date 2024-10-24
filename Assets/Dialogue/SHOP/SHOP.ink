EXTERNAL ShowBuyMenu()
EXTERNAL ShowSellMenu()

-> main

=== main ===
Wow, it seems we meet yet again!

What would you lilke to do?
    + [Buy]
        -> buy_UI
    + [Sell]
        -> sell_UI
    + [Bye]
        -> end
        
=== buy_UI ===
Here is what I currently have in stock!
~ ShowBuyMenu()
-> END

=== sell_UI ===
Show me what you'd like to sell.
~ ShowSellMenu()
-> END

=== end ===
Hopefully, this won't be the last time I see you alive!
-> END