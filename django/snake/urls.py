from django.conf.urls.defaults import *
from django.conf import settings

urlpatterns = patterns('views',
    url(r'^$', 'frontpage', name='frontpage'),
    url(r'^lobby$', 'lobby', name='lobby'),
    url(r'^target$', 'target', name='target'),
    url(r'^you$', 'you', name='you'),
    url(r'^you_touch$', 'you_touch', name='you_touch'),
    url(r'^wait_result$', 'wait_result', name='wait_result'),
    url(r'^result$', 'result', name='result'),

    # admin stuff
    url(r'^status$', 'status', name='status'),
    url(r'^reset/(?P<number_of_players>\d+)$', 'reset', name='reset'),
)

urlpatterns += patterns('django.views.static',
    (r'^media/(?P<path>.*)$', 'serve', {'document_root': settings.MEDIA_ROOT}),
)
