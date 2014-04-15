<?php
function safe_md5($file)
{
  if(!(file_exists($file))) { echo "file_error"; die; }
  return md5_file($file);
}

$install_data = safe_md5("./install-data.json", false);

$ch = curl_init();
curl_setopt($ch, CURLOPT_URL, 'http://localhost/status/json/config,status/');
curl_setopt($ch, CURLOPT_RETURNTRANSFER, 1);
$combined = curl_exec($ch);

echo (md5($combined) . "," . $install_data);

?>