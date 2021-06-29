This is a windows form/dialog that shows the current date & time along with the time zone. Additional time zones can be added. The form has no title bar, but it can be dragged around anywhere.

This version targets .NET Framework 6.0 because this is the framework that originally shipped with Windows 10.

Something I threw together because I wanted to know the current time. And in multiple time zones. And just because.

New 2.1 features (yeah!)
By popular demand, the year has been changed from 4 digits to 2 digits. It was very surprising that 2 digits takes up less space than 4. I am forming a select committee on spacy digits to discuss this bizarre behavior over drinks, many drinks. We will get back to you after a lengthy discussion.

Seconds can be turned off. I know everyone loves to stare at the raw evidence of the time traveling into the future we all do, 1 second at a time. But many of you (you know who you are) want a bland non flashy display that requires even less space. Well, you have it. Just drop the menu down and turn off the second’s display. After a short internal argument about the benefits of the flashy seconds display the program will obey your command. And just like that *poof* the seconds are gone as if they were never there in the first place. 

I also went through the code and took out the trash. I threw away some un-needed lines of code that were hiding in the mouse event handlers. They were not happy, and these lines are looking for a new place to live. All they need is a friendly event handler. They are down at the house of unused code. If you know a good home, please pick them up soon before they end up in the seedy part of town, working the street causing all kinds of exceptions.

Future plans include saving the very special list of selected time zones, and saving the window position. So make sure you fill out that registration card, put a stamp on it and mail it off to keep up with the latest and greatest goings on here at ItsAClock. Every 31418th card received will get a special gift. Maybe even a fruit basket. Who knows? I sure don't. 
