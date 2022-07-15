require random.fs

: print-card-value ( n -- n ; prints card value corresponding to index )
   dup 0 = if ." A"
   else dup 10 = if ." J"
   else dup 11 = if ." Q" 
   else dup 12 = if ." K" 
   else 1+ . then then then then ;

\ TODO: Don't really understand this code - understand it!
: time ( -- t ; Get system time: https://www.rosettacode.org/wiki/System_time#Forth )
    cputime d+ 1000 um/mod nip ;

\ time seed !       \ Init seed with timestamp NOT WORKING!
1234 seed !

cr ." ACEY-DUCEY IS PLAYED IN THE FOLLOWING MANNER"
cr ." THE DEALER (COMPUTER) DEALS TWO CARDS FACE UP"
cr ." YOU HAVE AN OPTION TO BET OR NOT BET DEPENDING"
cr ." ON WHETHER OR NOT YOU FEEL THE CARD WILL HAVE"
cr ." A VALUE BETWEEN THE FIRST TWO."
cr ." IF YOU DO NOT WANT TO BET, INPUT '0'"
cr ." YOU NOW HAVE 100 DOLLARS."
cr 
cr ." HERE ARE YOUR NEXT TWO CARDS:"
cr 11 random print-card-value                  \ lower card of range
cr 11 over - random over + print-card-value    \ upper card of range
cr
cr ." WHAT IS YOUR BET?"