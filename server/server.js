const express = require('express')
const app = express()
const port = 3000

var session = {}
var idle = true

app.use(express.json()); 

app.put('/', (request, response) => {
    processRequest(request.body)
    response.send(session)
})

app.listen(port, (err) => {
    if (err) {
        return console.log('something bad happened', err)
    }
    console.log('server is listening on ', port)
})

function processRequest(data){
   	session.host = false
    switch(data.command){
        case 1: // Join Game
            if(idle){
               session = {}    
               idle = false
               session.host = true
            }else{
                session.state = 1
            }
            break;

        case 2: // Wait for second player
            break;

        case 3: // Update game data
        	if(data.host){
	            session.host_position = data.host_position
	            session.ball_position = data.ball_position

	            session.ball_speed = data.ball_speed
	            session.ball_direction = data.ball_direction
	            session.ball_radius = data.ball_radius
        	}else{
	            session.guest_position = data.guest_position
        	}
            break;

        case 4: // Update gamestate
	    	session.state = data.state 
        	switch(data.state){
	        	case 5:
    	            idle = true                
            		break;

	    	}
        	break;
    
    }
    console.log(session)
}