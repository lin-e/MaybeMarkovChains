<?php
  $term = $argv[1];
  $url = "https://en.wikipedia.org/w/api.php?format=json&action=query&prop=extracts&exintro=&explaintext=&titles=".urlencode($term);
  $raw = file_get_contents($url);
  $decoded = json_decode($raw);
  foreach ($decoded->query->pages as $page) {
    echo stripcslashes($page->extract);
  }
  echo "\n";
?>
