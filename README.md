![basic1](https://github.com/devglassmeyer/ItsAClock/blob/main/img/basic1.jpg)

This is a windows form/dialog that shows the current date & time along with the time zone. Additional time zones can be added. The form has no title bar, but it can be dragged around anywhere.
This version targets .NET Framework 4.6 because this is the framework that originally shipped with Windows 10.
This is something I threw together because I wanted to know the current time. And in multiple time zones at a glance. And just because.
New 3.0 – Full of Charm!
When I think of this release, I think big. Very Big. Huge. Work on this release began years ago by an infinite number – 2 of monkeys typing away for an infinite amount of time – 14 days. Because the budget wasn’t there for an infinite number of monkeys for an infinite amount of time this isn’t Shakespeare. It’s a new and very much improved Clock. This version brings exciting new features.

Your time zone selections are not saved! As in written to disk. The time zone selection page has also changed. You can select to include a time zone in the saved list, show or not show the time for a time zone, and use a custom name for a time zone.

![selecttz](https://github.com/devglassmeyer/ItsAClock/blob/main/img/selecttz.jpg)

There is a new save dialog were you can specify a file, save, load, delete. Cool, right? 

![savesettings](https://github.com/devglassmeyer/ItsAClock/blob/main/img/savesettings.jpg)

On the main clock display when using a custom name for a time zone right clicking the time will show a menu with the custom name and actual time zone name. This menu allows you to switch between viewing the custom name and the time zone name.

![right-click](https://github.com/devglassmeyer/ItsAClock/blob/main/img/right-click.jpg)


New 2.1 features (yeah!)
By popular demand, the year has been changed from 4 digits to 2 digits. It was very surprising that 2 digits takes up less space than 4. I am forming a select committee on spacy digits to discuss this bizarre behavior over drinks, many drinks. We will get back to you after a lengthy discussion.

Seconds can be turned off. I know everyone loves to stare at the raw evidence of the time traveling into the future we all do, 1 second at a time. But many of you (you know who you are) want a bland non flashy display that requires even less space. Well, you have it. Just drop the menu down and turn off the second’s display. After a short internal argument about the benefits of the flashy seconds display the program will obey your command. And just like that *poof* the seconds are gone as if they were never there in the first place. 

I also went through the code and took out the trash. I threw away some un-needed lines of code that were hiding in the mouse event handlers. They were not happy, and these lines are looking for a new place to live. All they need is a friendly event handler. They are down at the house of unused code. If you know a good home, please pick them up soon before they end up in the seedy part of town, working the street causing all kinds of exceptions.

Future plans include saving the very special list of selected time zones, and saving the window position. So make sure you fill out that registration card, put a stamp on it and mail it off to keep up with the latest and greatest goings on here at ItsAClock. Every 31418th card received will get a special gift. Maybe even a fruit basket. Who knows? I sure don't.
