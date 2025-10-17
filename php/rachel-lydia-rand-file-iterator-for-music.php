<script type="text/javascript">
	// the iterator!!
	<?php
	
	$recordArray = array();
	
	function readTheDir($dirStr, $readableName = '', $skipDirectories = false) 
	{
		//echo $dirStr; exit;
		// this function isn't used any more, but is good for reference
		$baseDir = '';// for the readable name
		$dirResource = opendir($dirStr);
		$firstFile = false;
		$returnArray = array();
		while($entryName = readdir($dirResource)) 
		{
			if(substr($entryName, 0, 1) == '.') continue;
			if($entryName == 'index.php') continue;
			if(strpos($entryName, '.html') !== false || strpos($entryName, '.zip') !== false) continue;
			
			if(is_dir($dirStr . '/' . $entryName)) 
			{
				//echo '<hr />';
				array_push($returnArray, readTheDir($dirStr . '/' . $entryName, $readableName . '/' . $entryName, true));
			}
			else 
			{	
				// only allow mp3 and wav files
				if(strpos(strtolower($entryName), '.mp3') > -1 || strpos(strtolower($entryName), '.wav') > -1) 
				{
					if(!$firstFile)
					{
						$firstFile = true;
					}
					else
					{
						//echo '<br />';
					}
					
					array_push($returnArray, $readableName . '/' . $entryName);
				}
			}
		}
		closedir($dirResource);
		return $returnArray;// returns multidimensional array
	}
	
	
	$cwd = getcwd();
	
	
	echo "\r\n\r\n";
	
	function parseMultiDimArray($arr) {
		foreach($arr as $record) {
						
			if(is_array($record))
			{
				parseMultiDimArray($record);
				continue;
			}
			//if(strlen($record) < 15) continue;
			echo 'songs.push("' . str_replace('"', '&quot;', $record) . '");';
			//echo "Hit!";exit;
						
		}
	}
	
		
	$myMultiDimArray = readTheDir($cwd . '\\music\\', '/for/music');
	parseMultiDimArray($myMultiDimArray);
	//$myMultiDimArray = readTheDir($cwd . '/musicFolder', '/musicFolder');
	//parseMultiDimArray($myMultiDimArray);
	
	
	echo count($myMultiDimArray);
	
	
	
	
	
	//echo '</script><br><br><br>-----------<br><script>';
	
?>
	
	
	search();
	
</script>