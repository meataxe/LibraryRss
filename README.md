LibraryTitles
=============

A rough front-end over the PNCC library new titles RSS feed.

Created because the oldies sometimes hide the new books to reserve when they come out,
and to show up the other new books that don't have their own "new titles" shelf like
the adults fiction section does.

This attempts to parse the rss feed, visit the page for each item on it, pull some extra 
data about the title and use it to rate the thing on how likely I am to want to reserve 
it using basic black- and white-lists for keywords in the subject or section fields.

The source feed is here:
https://ent.kotui.org.nz/client/rss/hitlist/pn/qu=newbks-pn&dt=list&st=PD

The code is running on appharbor (maybe, if it's currently compiling ok) here:
http://pnlibrarytitles.apphb.com
