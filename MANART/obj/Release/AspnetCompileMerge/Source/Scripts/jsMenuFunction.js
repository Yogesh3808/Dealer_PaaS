function LoadMenuFromXML(xmlFile) 
{ 	
    var xmlDoc=new ActiveXObject("Microsoft.XMLDOM");
    //'XmlFiles/Test.xml'

    xmlDoc.async="false"; 	
    //xmlDoc.onreadystatechange=verify; 					
    xmlDoc.load(xmlFile); 					
    xmlObj=xmlDoc.documentElement; 	    
}
