get.data <- function(path) 
{
  dataset <- read.transactions(path, format = "basket", sep = ",", quote = NULL, encoding = "UTF-8");
  return(dataset);
}

get.plots <- function(foldername, rulesSource)
{
  plot(ruleSource, measure=c("support", "lift"), shading="confidence");
  plot(ruleSource);
  head(quality(ruleSource));
  plot(ruleSource, shading="order", control=list(main="Two-key plot"));
  sel <- plot(ruleSource, measure=c("support", "lift"), shading="confidence", interactive=TRUE);
  subrulesByLift <- head(sort(ruleSource, by="lift"), 10);
  subrulesByConfidence <- head(sort(ruleSource, by="confidence"), 10); 
  subrulesBySupport <- head(sort(ruleSource, by="support"), 10);  
}

generate.file.name <- function(algorithmTypeName)
{
  filename <- "";
  
  #Uncomment these rows to add random session identificator 
  #otherwise it generates data to the same file each time 
  
  #documentid <- as.character(sample(1:99999999, 1));
  filename <- paste(algorithmTypeName,"");
  algorithmName <- paste(algorithmTypeName, "result", sep="#");
  #filename <- paste(algorithmName, documentid, sep ="#");
  filename <- paste(writepath, filename, sep = " ");
  filename <- paste(filename,".csv", sep = " ");
  
  return(filename)
}

setwd("C:\\Users\\Vladislav\\Desktop\\job_skills_analytics\\Apriori\\bin\\Debug\\RSCRIPT\\");
path <- "C:\\Users\\Vladislav\\Desktop\\job_skills_analytics\\Apriori\\bin\\Debug\\RSCRIPT\\dataset.csv";
writepath <- "C:\\Users\\Vladislav\\Desktop\\job_skills_analytics\\Apriori\\bin\\Debug\\RSCRIPT\\";

inputSup <- 0.02;
inputConf <- 0.1;
maxlenEclat <- 15;
transactions <- get.data(path);

#aprioriRules <- calculate.rules.apriori(inputSup, inputConf, transactions);
aprioriRules<- apriori(transactions, parameter=list(supp = inputSup, conf=inputConf, target="rules"));
eclatRules <- eclat(transactions, parameter = list(supp = inputSup, maxlen = maxlenEclat));
  
filenameApriori <- generate.file.name("APRIORI");
write(aprioriRules, file = filenameApriori, sep="", row.names = FALSE);

#as(rules, "data.frame");
#write.csv(rules[,"lhs","rhs","support","confidence","lift"], file=filename, row.names = FALSE);

filenameEclat <- generate.file.name("ELCAT");

write(eclatRules, file=filenameEclat,sep="", row.names = FALSE);

filenameEclat = "";
filenameApriori = "";

gc();
