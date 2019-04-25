const express = require('express');
const app = express();
const cors = require("cors");
var myParser = require("body-parser");
var fs = require('fs');
var parse = require('csv-parser');

const fetchCommentPage = require('youtube-comment-api')

var log_likelihoods = JSON.parse(fs.readFileSync('log_likelihood.json', 'utf8'));
var log_prior = [-0.8315409087605821,-0.571563];
var unique_words = [];

/*
Read the unique words from V.csv and save them in the varible unique_words
*/
fs.createReadStream("V.csv")
    .pipe(parse({delimiter: ','}))
    .on('data', function(csvrow) {
        unique_words.push(csvrow.words);
    })
    .on('end',function() {
    });


app.use(express.static('public'));
app.use(myParser.urlencoded({extended : true}));

app.get('/', function(request, response) {
  response.sendFile(__dirname + '/views/index.html');
});



/*

*/
app.post('/sentiment_analysis',cors(),function(request,response,next){
  var video_id = request.body.video_id;
  var num_of_comments = request.body.num_of_comments;
  var final_answers = [];

  //Getting the comments initially
  fetchCommentPage(video_id).then(commentPage => {
    var comments = commentPage.comments;

    for(var i = 0; i < comments.length; i++){
      var comment = comments[i].text;
      final_answers.push(get_sentiment(comment));
    }

    return fetchCommentPage(video_id, commentPage.nextPageToken)
  })


  //final comment to get
  .then( commentPage => {
    var comments = commentPage.comments;

    for(var i = 0; i < comments.length; i++){
      var comment = comments[i].text;
      final_answers.push(get_sentiment(comment));
      console.log(get_sentiment(comment)+" => "+comment);
    }

    var pos_count = 0;
    for(var i = 0; i < final_answers.length; i++){
      if(final_answers[i]==1){
        pos_count++;
      }
    }
    var pos_ratio = pos_count/final_answers.length;

    response.status(200);
    response.send({"answer":pos_ratio});
  });

});


function get_sentiment(data){
  var log_posteriors = [];
  var final_answer = 1;
  var log_posterior = get_log_posterior(data);
  log_posteriors.push(log_posterior[0]);
  log_posteriors.push(log_posterior[1]);
  if(log_posterior[0]>log_posterior[1]){
    final_answer = 0;
  }
  return final_answer;
}



function get_log_posterior(data){
  data = preprocess(data);

  //naive bayes inference
  var classes = [0,1];
  var log_posterior = [];

  for(var ci = 0; ci < classes.length; ci++){
    var log_likelihood_sum = 0;
    for(var i = 0; i < data.length; i++){
      var word = data[i];
      if(unique_words.indexOf(word)!=-1){
        log_likelihood_sum += log_likelihoods[ci][word];
      }
    }
    log_posterior.push(log_prior[ci]+log_likelihood_sum);
  }
  return log_posterior;
}


/*
Preprocesses a string, it keeps only english characters and numbers and returns an array of words
Arguments:
  data: string to preprocess
Returns:
  An array containing the words in the original string after applying preprocessing
*/
function preprocess(data){
  data = data.toLowerCase().replace(/[^a-zA-Z0-9]+/g, " ");
  return data.split(" ");
}


/*
This function replaces all occurences of a substring by another substring
Arguements:
  str: Original string
  find: substring to search for
  replace: substring to put in the place of find
Returns:
  Modified string
*/
function replaceAll(str, find, replace) {
    return str.split(find).join(replace);
}


// listen for requests
const listener = app.listen(process.env.PORT, function() {
  console.log('Analyzo is listening on port ' + listener.address().port);
});
