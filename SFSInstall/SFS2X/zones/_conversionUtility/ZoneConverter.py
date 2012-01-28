import os, fnmatch, sys

floodFilter = {}
floodFilter["open"] = "<floodFilter"
floodFilter["close"] = "</floodFilter>"
floodFilter["replacement"] = "<floodFilter active='false'><banDurationMinutes>1440</banDurationMinutes><maxFloodingAttempts>5</maxFloodingAttempts><secondsBeforeBan>5</secondsBeforeBan><banMode>NAME</banMode><logFloodingAttempts>true</logFloodingAttempts><banMessage>Too much flooding, you are banned.</banMessage></floodFilter>"

refactorings = [floodFilter]


def processFiles():
	zoneFiles = [item for item in walkDir('.', False, '*.zone.xml')]
	
	for zoneFile in zoneFiles:
		content = refactorZoneFile(zoneFile)
		newZoneFile = open(zoneFile + '.new', 'w')
		newZoneFile.write(content)
		newZoneFile.close()

def refactorZoneFile(zoneFile):
	theFile = open(zoneFile, 'r')
	content = theFile.read()
	print "Processing: ", zoneFile

	for refactorObj in refactorings:
		startPos = content.find(refactorObj["open"])
		endPos = content.find(refactorObj["close"])
		
		if (startPos < 0) or (endPos < 0):
			print "\tProcessing failed!. Refactoring: %s, Start: %s, End: %s" % (refactorObj["open"], startPos, endPos)
			break
	
		# Apply refactoring
		content = content[0:startPos - 1] + refactorObj["replacement"] + content[endPos + len(refactorObj["close"]):]
		
	return content
	
	
#
# Walk the directory structure recursively
#
def walkDir(root='.', recurse=True, pattern='*'):
	for path, subdirs, files in os.walk(root):
		for name in files:
			if fnmatch.fnmatch(name, pattern):
				yield os.path.join(path, name)

		if not recurse:
			break	
	
def main():
	processFiles()

if __name__ == '__main__':
	main()