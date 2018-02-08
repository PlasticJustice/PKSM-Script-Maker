#!/usr/bin/env python3
import shlex, PKSMScript, sys, glob, shutil, os

games = ["PSM"]

def main(args):
	shutil.rmtree("output", True)
	os.mkdir("output")
	for game in games:
		generate(game)
		
		scriptFiles = glob.glob("*.pksm")
		for pksmFile in scriptFiles:
			shutil.move(pksmFile,"output")
	os.remove("scriptsPSM.txt")	
	os.remove("PKSMScript.py")
	os.remove("genScripts.py")
	shutil.rmtree("__pycache__", True)
	

def generate(game):
	with open(os.path.join("scripts%s.txt" % game)) as pksmArgFile:
		for line in pksmArgFile:
			if (not line.startswith('#')):
				line.replace('\\', '/')
				pksmArgs = PKSMScript.parser.parse_args(shlex.split(line))
				PKSMScript.main(pksmArgs)

if __name__ == '__main__':
	main(sys.argv)
