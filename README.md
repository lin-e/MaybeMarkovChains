# MaybeMarkovChains
This probably isn't how Markov chains work, but I find it humorous nonetheless.

The original Python code this is based off can be found [here](http://eflorenzano.com/blog/2008/11/17/writing-markov-chain-irc-bot-twisted-and-python/).

I don't plan to maintain this code, and I made it because I was bored - please do not judge my ability off of some code that I ported and slightly modified from a 8-9 year old Python script.

# Usage
This was made to be used on a Raspberry Pi Zero W (because I had one lying around). The main code was compiled as an .exe and run with mono:
```
mono ChainChat.exe <true|false - whether it should learn phrases> <chain length>
```
In order to speed up training, I have included some PHP scripts to fill up the file (formatting is horrible, I don't write PHP, and I really didn't care about the presentation of this piece).
### Donald Trump's Twitter
```
touch brain.txt
php trump.php >> brain.txt
```
### Wikipedia Summaries
```
touch brain.txt
php wiki.php "<search term>" >> brain.txt
```
