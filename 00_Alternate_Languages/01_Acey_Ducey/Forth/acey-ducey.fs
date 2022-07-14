require random.fs

: time ( -- t ; Get system time: https://www.rosettacode.org/wiki/System_time#Forth )
    cputime d+ 1000 um/mod nip ;

( time seed ! -- Init seed with timestamp NOT WORKING! )
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
( TODO: Show face cards A,J,Q,K properly )
cr 13 random 1+ .
cr 13 random 1+ .
cr
cr ." WHAT IS YOUR BET?"