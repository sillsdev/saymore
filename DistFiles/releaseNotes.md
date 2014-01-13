#What's New in Version 3

##IMDI Archiving

SayMore users contributing to archives can now export SayMore data to a folder that conforms to  [IMDI](http://tla.mpi.nl/imdi-metadata/) format. You can then open the folder in IMDI applications like [ARBIL](http://tla.mpi.nl/tools/tla-tools/arbil/) for further annotation and submission.

##Project Level Metdata

SayMore 3 adds a "Project" tab for the first time, along with these Project-level fields:

* Title
* Funding Project Title
* Description
* Vernacular Language
* Location/Address
* Region
* Country
* Continent
* Contact Person
* Content Type
* Applications
* Date Available
* Rights Holder
* Depositor
* Releated Publications

##New Session Fields

 * Interactivity
 * Involvement
 * Location country
 * Location continent
 * Location region
 * Location address
 * Sub-Genre
 * Interactivity
 * Planning Type
 * Involvement
 * Social Context
 * Task

##New Person Fields

 * Nick Name
 * Privacy Protection

##New Person Tab: Contributions

 * The Contributions Tab automatically lists each session where the person is listed as a participant, and each file where they have been assigned a role under the file's _Contributors_ tab.

##Standard Access Protocols

In previous versions of SayMore, you could type whatever you like in the "Access" field. Now, you have three options:

 1. You can choose from the [FURCS](http://www.elar-archive.org/using-elar/access-protocol.php) or "REAP" Access Protocols, and SayMore will restrict your choices to the their closed vocabulary.

 1. You can set up a custom set of choices.

 1. Or you can still just let yourself type in whatever you want, as with version 2.

##Thanks
Sarah Moeller and Tim Gaved served as consultants and _champions_ for this release.

# What's New in Version 2

Version 1 of SayMore helped you get everything organized. Version 2 add many  tools for helping you record, transcribe, convert, export, and archive.  Optionally, you can also enlist native speakers to do time-aligned "oral  annotation" steps, as used by the Basic Oral Language Documentation approach. In what follows, we summarize the changes made since Version 1:

You can now record new sessions directly in SayMore.

> _We <strong>plead</strong> with you to avoid using your laptop's built-in microphone; if you  are willing to give up usefulness for future phonetic research, an OK USB  headset can be had for US$35 or less in many countries. Note that the very  popular Zoom H2's can also be plugged in and used as a microphone (in a lower  quality mode)._

All recording tools now feature a level meter which also indicates which device  SayMore is recording from, to reduce the chances that you think you're recording  from, say, a headset, but really the computer is listening to the laptop's  built-in microphone.

* SayMore now has an easy-to-use transcription tool, which looks like a table.  Each row is a "segment". Before you can transcribe, you need to make these  segments, and you can do that in one of 3 ways:

	1) If you're doing BOLD, just set a native speaker up with the CarefulSpeech  tool. Each chunk of speech he/she repeats becomes a segment.

	2) You can use the Manual Segmenter

	3) Try our experimental Auto-segmenter

* In the Transcription tool, you can now choose what audio is played when the  cursor is in the row. Original recording, careful recording or oral translation.

##Oral Annotation

The Careful Speech and Oral Translation tools, which support the BOLD way of  working, are designed to be super simple so that you can quickly train a native  speaker to do these tasks:

* A single key (space bar) is all that is needed to do the whole task.
* If there are segments you don't want to annotate, you can mark them as  "ignored".

##File Format Handling
Many audio and video formats can now be converted to standard, future-friendly  formats, right in SayMore.

##Export
In addition to the ELAN file compatibility, Audacity import, and FLEx export,  this version adds the following exports:

* Plain text
* CSV (comma-separated-value) for spreadsheets
* SFM (Toolbox)
* SRT Subtitle format

##MetaData
As before, SayMore tries to figure out which recordings have gone through which  stages of the workflow. If it gets it wrong, then you can use the new "status  and Stages" tab can now manually take control of the stage for each session.

### New features in version 1.5:

Built-in "Careful Speech" and "Oral Translation" tools for those  doing BOLD (Basic Oral Language Documentation).

### New features in version 1.3:

Built-inTranscription, stored using ELAN's file format, with export for  FieldWorks Language Explorer (FLEx) interlinear.

### New features in version 1.1:

Built-in packaging for REAP, SIL's Corporate Archive


### New features in version 0.9:

Documentation.(thanks Marlon!)

You can control which columns are shown, in what order, and you can sort by clicking on the header of the column.

You can now record exactly who did what, when, using the Contributors tab.


### New features in version 0.7:

Progress now shows nice charts. You can print these direct, or copy them to an email or report.

### New features in version 0.5:

Videos are now shown with a preview image, rather than a black rectangle, when not playing. When you correct the spelling of a person's name, the name is also updated in all events where he/she is listed as a participant.

The People list now gives an indication of the Informed Consent status of each person. You get a warning icon if there is no consent file, and different icons for photograph, audio and video consent files.

Events now have a "Status" value, which you set. Currently, the choices are "Incoming", "In Progress", "Finished", and "Skip". The later is for events which you don't plan to work with or publish.

The list of Events now displays information about the workflow status in the "Stages" column. As you add files to the event and name them appropriately (using the Rename menu), SayMore detects them and indicates their presence in the stages icon.

You can now select participants for an event by ticking the boxes next to their names. You can now set the mug shot of a person by dragging the photo file onto the mug shot place-holder.


### Known Problems

Some users are finding they cannot rename media files, because the process which  shows them (MediaPlayer.exe) is holding on to them when it should not.&nbsp;  Apparently a symptom of this is that multiple MediaPlayers can be found running  in the Windows process list.&nbsp; However, we have not yet been able to make  this happen on developer machines, which makes it very hard to fix.&nbsp; If you  can reliably make it happen, please contacting us!

### Known Limitations

The Export to CSV command is only partially implemented.

SayMore doesn't provide any help with Informed Consent/permission situations where someone gives universal access for some recordings, but limitted access for others.

There is no way yet to customize the file naming conventions.

When using the "Add Files..." button, you can only choose one file at a time.