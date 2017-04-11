<?php
  $years = array("2009", "2010", "2011", "2012", "2013", "2014", "2015", "2016", "2017");
  foreach ($years as $year) {
    $all = json_decode(file_get_contents("http://www.trumptwitterarchive.com/data/realdonaldtrump/$year.json"));
    foreach ($all as $single) {
      if ($single->is_retweet) {
        continue;
      }
      echo preg_replace("@(https?://([-\w\.]+[-\w])+(:\d+)?(/([\w/_\.#-]*(\?\S+)?[^\.\s])?).*$)@", " ", $single->text)."\n";
    }
  }
?>
