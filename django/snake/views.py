from django.http import HttpResponse, HttpResponseRedirect
from django.contrib.sessions.models import Session
from django.shortcuts import get_object_or_404, render_to_response, redirect
from django.core.urlresolvers import reverse
from django.views.decorators.cache import never_cache
from django.template.context import RequestContext
import random
import uuid

class Game():
    def __init__(self, number_of_players):
        self.players = []
        self.number_of_players = number_of_players
        self.snakes_created = False
        self.snakes = []
        self.has_result = False
        self.number_of_numbers = 10
        self.snakes_done = 0

class Snake():
    def __init__(self):
        self.player_indices = []
        self.position = -1
        self.untouchedplayers = -1

class Player():
    def __init__(self, name):
        self.name = name
        self.me = -1
        self.target = -1
        self.touched = False
        self.snake_index = -1

game = Game(2)
Session.objects.all().delete()
        
@never_cache
def frontpage(request):
    global game

    context = {}

    if len(game.players) >= game.number_of_players:
        context = { 'message': 'Snake Club is full. Sorry.'}

    if request.method == 'POST':
        if len(game.players) < game.number_of_players:
            name = request.POST['name']
            if name:
                name = name.strip()
                player = Player(name)
        
                game.players.append(player)
                request.session['player_index'] = len(game.players) - 1
        
                return HttpResponseRedirect(reverse('lobby'))
        
    if request.session.get('player_index', None):
        return HttpResponseRedirect(reverse('lobby'))
    else:
        return render_to_response('frontpage.html', context, RequestContext(request))
        
@never_cache
def lobby(request):
    global game
    
    if request.session.get('player_index', None) == None:
        return HttpResponseRedirect(reverse('frontpage'))
    
    if len(game.players) == game.number_of_players:
        if not game.snakes_created:
            make_snakes()
            
        return HttpResponseRedirect(reverse('target'))
    
    list_of_player_names = list(player.name for player in game.players)
    list_of_player_names.reverse()
    player_names = ', '.join(list_of_player_names)
    
    return render_to_response('lobby.html', { 
        'player_names': player_names, 
        'waiting_for': (game.number_of_players - len(game.players)) 
    }, RequestContext(request))
    
def make_snakes():
    global game

    snake_1_size = game.number_of_players / 2
    snake_2_size = game.number_of_players - snake_1_size
    
    random_player_indices = list(range(0, game.number_of_players))
    random.shuffle(random_player_indices);
    
    numbers = list(range(1, game.number_of_numbers + 1))
    random.shuffle(numbers)

    def assign_numbers(player_indices, snake_index):
        for i in range(0, len(player_indices)):
            player_index = player_indices[i]
            next_player_index = player_indices[(i + 1) % len(player_indices)]

            game.players[player_index].me = numbers[player_index]
            game.players[player_index].target = numbers[next_player_index]
            game.players[player_index].snake_index = snake_index
            
            print game.players[player_index].snake_index, ' : ', game.players[player_index].me, '->', game.players[player_index].target
            
    snake1 = Snake()
    snake1.player_indices = random_player_indices[0:snake_1_size]
    snake1.untouched_players = len(snake1.player_indices)
    assign_numbers(snake1.player_indices, 0)
    
    snake2 = Snake()
    snake2.player_indices = random_player_indices[snake_1_size:game.number_of_players]
    snake2.untouched_players = len(snake2.player_indices)
    assign_numbers(snake2.player_indices, 1)

    print str(snake1.player_indices), str(snake2.player_indices)

    game.snakes = [snake1, snake2]
    game.snakes_created = True
  
@never_cache 
def target(request):
    global game
    
    player_index = request.session.get('player_index', None)
    
    if player_index == None:
        return HttpResponseRedirect(reverse('frontpage'))

    player = game.players[player_index]

    return render_to_response('target.html', { 
        'animal_number': str(player.target)
    }, RequestContext(request))


@never_cache
def you(request):
    global game
    player_index = request.session.get('player_index', None)
    
    if player_index == None:
        return HttpResponseRedirect(reverse('frontpage'))
    
    player = game.players[player_index]
    
    return render_to_response('you.html', { 
        'animal_number': str(player.me)
    }, RequestContext(request))
    
@never_cache
def you_touch(request):
    player_index = request.session.get('player_index', None)
    
    if player_index == None:
        return HttpResponseRedirect(reverse('frontpage'))

    global game
    
    player = game.players[player_index]
    player.touched = True

    snake = game.snakes[player.snake_index]
    snake.untouched_players -= 1
    
    if snake.untouched_players == 0:
        snake.position = game.snakes_done + 1
        game.snakes_done += 1

    if len(list(player for player in game.players if player.touched)) == game.number_of_players:
        game.has_result = True
    
    return HttpResponseRedirect(reverse('wait_result'))

@never_cache
def wait_result(request):    
    if request.session.get('player_index', None) == None:
        return HttpResponseRedirect(reverse('frontpage'))
    
    global game
    
    if game.has_result:
        return HttpResponseRedirect(reverse('result'))
    
    return render_to_response('wait_result.html', {}, RequestContext(request))

@never_cache    
def result(request):
    if request.session.get('player_index', None) == None:
        return HttpResponseRedirect(reverse('frontpage'))
    
    global game
    
    sorted_snakes = sorted(game.snakes, key=lambda snake: snake.position)
    result = '<br/>'.join(('<h1>' + str(snake.position) + '. place:</h1>' + (', '.join(game.players[idx].name for idx in snake.player_indices))) for snake in sorted_snakes)
    
    return render_to_response('result.html', { 'result': result }, RequestContext(request))
    
def reset(request, number_of_players):
    global game
    
    game = Game(int(number_of_players))
    Session.objects.all().delete()
    
    return HttpResponse('Reset to ' + str(game.number_of_players) + ' players.')
    
def status(request):
    global game
    return HttpResponse('<h1>Status</h1><p>Players: ' + str(', '.join(player.name for player in game.players)) + '</p>')