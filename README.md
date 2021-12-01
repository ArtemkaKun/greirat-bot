# What?

This is a Discord bot, that helps us, 4Experience workers, automate food ordering process.

Features of a bot:

- Notifications about start and finish food voting
- Flexible orders list (CRUD operations with a few helper methods)

The bot was written with .NET 5 framework and Discord.NET library.

# Why?

Because of 4Experience continue growing and managing food orders every day is becoming more and more complicated task, I decided to create this bot for us. For now a bot can only do simple operations, but in the future it's planned to add more great features (like Pyszne.pl integration).

# How?

1. Create a bot on Discord Developers portal (or use existed one)
2. Copy it's token (or ask admin about that)
3. Add it to a Discord channel (or ask admin to add you to a developers channel)
4. Create an enviroment variable with the name **FOOD_BOT_TOKEN** and value - copied token, on your server/dev computer

If you not a developer and want just to use bot - build this solution and start the program (console window should appear).

# List of commands

|Command                                                      | Description                        | 
|-------------------------------------------------------------|-------------------------------------|
!help, !h                                                     | Shows bot's commands                | 
!order -mk <orderText>                                        | Creates a new order                 | 
!order -upd <idOfOrder> <newOrderText>                        | Updates order with provided text    | 
!order -del <idOfOrder>                                       | Deletes order                       | 
!shall                                                        | Shows all today's orders            | 
!shall -sort                                                  | Shows all today's orders (sorted)   | 
!shall -sum                                                   | Shows all today's orders (summary)  | 
!shmy                                                         | Shows your today's orders           | 
!shmy -sort                                                   | Shows your today's orders (sorted)  | 
!shmy -sum                                                    | Shows your today's orders (summary) | 
!voteReminder -set <remindTime> <remindMessage>               | Sets food vote reminder             | 
!voteReminder -sh                                             | Show channel's reminder data        | 
!voteReminder -del                                            | Delete channel's reminder           | 
!voteReminder -upd <remindTime> <remindMessage>               | Update channel's reminder           | 
!voteReminder -config <durationInMinutes> <voteFinishMessage> |                                     | 
