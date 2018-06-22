#!/usr/bin/env python3
import shlex, PKSMScript, sys, glob, shutil, os

games = ["PSM", "USUM", "SM", "ORAS", "XY", "B2W2", "BW", "HGSS", "PT", "DP"]

def main(args):
	if os.path.isdir("scripts"):
		for game in games:
			if os.path.isfile("scripts%s.txt" % game):
				generate(game)
				if game == "PSM":
					scriptFiles = glob.glob("*.pksm")
					for pksmFile in scriptFiles:
						if os.path.isfile("scripts\%s" % pksmFile):
							os.remove("scripts\%s" % pksmFile)
							shutil.move(pksmFile,"scripts")
						else:
							shutil.move(pksmFile,"scripts")
					os.remove("scriptsPSM.txt")
				else:
					scriptFiles = glob.glob("*.pksm")
					for pksmFile in scriptFiles:
						if os.path.isdir("scripts\%s" % game.lower()):
							if os.path.isfile("scripts\%s\%s" % (game.lower(), pksmFile)):
								os.remove("scripts\%s\%s" % (game.lower(), pksmFile))
								shutil.move(pksmFile, "scripts\%s" % game.lower())
							else:
								shutil.move(pksmFile, "scripts\%s" % game.lower())
						else:
							os.mkdir(game.lower())
							shutil.move(pksmFile,game.lower())
							shutil.move(game.lower(), "scripts")
					
					os.remove("scripts%s.txt" % game)
	else:
		os.mkdir("scripts")
		for game in games:
			if os.path.isfile("scripts%s.txt" % game):
				generate(game)
				if game == "PSM":
					scriptFiles = glob.glob("*.pksm")
					for pksmFile in scriptFiles:
						shutil.move(pksmFile,"scripts")
					os.remove("scriptsPSM.txt")
				else:
					os.mkdir(game.lower())
					scriptFiles = glob.glob("*.pksm")
					for pksmFile in scriptFiles:
						shutil.move(pksmFile,game.lower())
					shutil.move(game.lower(), "scripts")
					os.remove("scripts%s.txt" % game)

	os.remove("PKSMScript.py")
	os.remove("genScripts.py")
	if os.path.isdir("__pycache__"):
		shutil.rmtree("__pycache__", True)
	if os.path.isfile("g7wc.wc7"):
		os.remove("g7wc.wc7")
	if os.path.isfile("g6wc.wc6"):
		os.remove("g6wc.wc6")
	if os.path.isfile("g5wc.pgf"):
		os.remove("g5wc.pgf")
	if os.path.isfile("g4wc.pgt"):
		os.remove("g4wc.pgt")
	if os.path.isfile("binary.bin"):
		os.remove("binary.bin")
	if os.path.isfile("text.txt"):
		os.remove("text.txt")
	if os.path.isfile("pkm4.smk4"):
		os.remove("pkm4.smk4")
	if os.path.isfile("pkm5.smk5"):
		os.remove("pkm5.smk5")
	if os.path.isfile("pkm6.smk6"):
		os.remove("pkm6.smk6")
	if os.path.isfile("pkm7.smk7"):
		os.remove("pkm7.smk7")	
	if os.path.isfile("g4pcd.pcd"):
		os.remove("g4pcd.pcd")	
	
def generate(game):
	with open(os.path.join("scripts%s.txt" % game)) as pksmArgFile:
		for line in pksmArgFile:
			if (not line.startswith('#')):
				line.replace('\\', '/')
				pksmArgs = PKSMScript.parser.parse_args(shlex.split(line))
				PKSMScript.main(pksmArgs)

if __name__ == '__main__':
	main(sys.argv)
