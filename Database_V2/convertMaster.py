from MainFunctions import *
from Functions import wytesong,Tierlist
import ParamFunctions

PATH_convert=os.path.join(os.path.dirname(os.path.realpath(__file__)),'_converted1')
PATH_convert2=os.path.join(os.path.dirname(os.path.realpath(__file__)),'_converted2')
PATH_export=os.path.join(os.path.dirname(os.path.realpath(__file__)),'export')

def convertRaws(save=False):
    masters=loadFiles(
    ['MasterParam.json', 'MasterParamJP.json', 'QuestParam.json','QuestParamJP.json', 'QuestDropParam.json'])
    #loc = Translation()
    export={}
    #some tries first
    i=0
    for master in masters:
        for main in master:
            #get right method for convertion
            mName=main[0].upper()+main[1:]
            method= getattr(ParamFunctions, mName+'Param',  getattr(ParamFunctions, mName+'EffectParam',getattr(ParamFunctions, mName[:-1]+'Param',False)))
            if not method:
                print('Not Found: '+main)
                continue

            #convert the main
            #print(main)
            converted={}
            if 'iname' in master[main][0]:#most Params
                converted={
                    entry['iname']:method(entry)
                    for entry in master[main]
                    }
            else: #like Tobira
                converted=[
                    method(entry)
                    for entry in master[main]
                    ]
                if len(converted)==1:
                    converted=converted[0]

            #add to new main
            if main in export:
                if type(converted)==dict:
                    if type(list(converted.items())[0][1])==dict:
                        for item in converted:
                            if item in export[main]:
                                export[main][item]['dif']=DifParam(export[main][item],converted[item])
                            else:
                                export[main][item]=converted[item]
                    else:
                        export[main]['dif']=DifParam(export[main],converted)
                elif type(converted)==list:
                    pass
                print('Fused '+main)
            else:
                export[main]=converted

    if save:
        for main in export:
            saveAsJSON(PATH_convert, main+'.json',export[main])
    return export

def Fixes(master):
    UNIT=master['Unit']
    SKILL=master['Skill']
    BUFF=master['Buff']
    ITEM=master['Item']
    JOB=master['Job']

     ##job############################################################
    print('Fix - Job')
    if 1:
        #abilities and stats
        for key,job in master['Job'].items():
            if 'fixed_ability' not in job:
                continue
            #abilities
            abilities={
                'main': job['fixed_ability'],
                'sub' : '',
                'reaction': [],
                'passive': []
            }
            for abil in job['abilities']:
                slot = master['Ability'][abil]['slot']
                if slot=='Action':
                    abilities['sub']=abil
                elif slot == 'Reaction':
                    abilities['reaction']+=[abil]
                elif slot == 'Support':
                    abilities['passive']+=[abil]

            job['abilities']=abilities
            del job['fixed_ability']

            #stats
            stats={}
            for i,rank in enumerate(job['ranks']):
                if i == 0:
                    continue
                for item in rank['equips']:
                    for buff in BUFF[SKILL[ITEM[item]['skill']]['target_buff_iname']]['buffs']:
                        if buff['type'] not in stats:
                            stats[buff['type']]=0
                        if i<len(job['ranks'])-1:
                            stats[buff['type']]+=buff['value_ini']
                        else:
                            stats[buff['type']]+=buff['value_max']
            job['stats']=stats

        #save
        saveAsJSON(PATH_convert2, 'Job.json',JOB)
    ##quests#########################################################
    print('Fix - Quests')
    if 1:
        #load in scene and set
        PATH_maps=os.path.join(PATH,*['resources','GameFiles','LocalMaps'])
        for key,quest in master['quests'].items():
            #maps
            if 'map' in quest:
                for map in quest['map']:
                    if os.path.exists(os.path.join(PATH_maps, map['mapSceneName'])):
                        with open(os.path.join(PATH_maps, map['mapSceneName']), "rt", encoding='utf-8-sig') as f:
                            map['Scene']=(json.loads(f.read()))
                    if os.path.exists(os.path.join(PATH_maps, map['mapSetName'])):
                        with open(os.path.join(PATH_maps, map['mapSetName']), "rt", encoding='utf-8-sig') as f:
                            map['Set']=(json.loads(f.read()))   
                        party=['party','enemy']
                        for p in party:
                            if p in map['Set']:
                                for index,value in enumerate(map['Set'][p]):
                                    map['Set'][p][index] = ParamFunctions.MapSetting(value)
            #drop list
        #save
        saveAsJSON(PATH_convert2, 'Quests.json',master['quests'])

    ##units##########################################################
    print('Fix - Unit')
    if 1:
        # jobset to job and add skins and seperate unit and NPC
        (eUnit, eEnemy)=({},{})
        for key,unit in UNIT.items():
            #unit or enemy
            if 'lore' in unit or ('piece' in unit and 'ai' in unit and unit['ai']=='AI_PLAYER' and 'EN' != key[6]+key[7] and 'role' not in unit):
                eUnit[key]=unit
            else:
                eEnemy[key]=unit

            #add skins from dif
            if 'dif' in unit and 'skins' in unit['dif']:
                unit['skins']+=unit['dif']['skins']

            #change js to job
            if 'jobsets' in unit:
                unit['jobs']=[
                    master['JobSet'][js]['job']
                    for js in unit['jobsets']
                    ]
                del unit['jobsets']

        #add unit kanji translations via wytesong
        wytesong(eUnit)

        #add tierlist
        Tierlist(eUnit)

        #add job+
        for key,js in master['JobSet'].items():
            if 'jobchange' in js:
                unit = UNIT[js['target_unit']]
                if 'jobchanges' not in unit:
                    unit['jobchanges']=[None]*len(unit['jobs'])

                for index,job in enumerate(unit['jobs']):
                    if job == js['lock_jobs']['iname']:
                        unit['jobchanges'][index]=js['job']

        #add kaigan
        for kaigan in master['Tobira']:
            unit = UNIT[kaigan['mUnitIname']]
            if 'kaigan' not in unit:
                unit['kaigan']={}
            unit['kaigan'][kaigan['mCategory']]=kaigan

        #add nensou
        for key,card in master['ConceptCard'].items():
            if 'effects' not in card:
                continue

            for effect in card['effects']:
                if 'cnds_iname' not in effect:
                    continue

                conds = master['ConceptCardConditions'][effect['cnds_iname']]
                if 'unit_group' not in conds:
                    continue
                units = master['UnitGroup'][conds['unit_group']]['units']

                if len(units)==1:
                    #skin
                    if 'skin' in effect:
                        UNIT[units[0]]['skins'].append(effect['skin'])
                    #normal stuff
                    UNIT[units[0]]['conceptcard']=key

        #add occurence
        for key,quest in master['quests'].items():
            if 'map' in quest:
                for map in quest['map']:
                    if 'Set' in map and 'enemy' in map['Set']:
                        for enemy in map['Set']['enemy']:
                            if enemy['iname'] in UNIT:
                                if 'occurrence' not in UNIT[enemy['iname']]:
                                    UNIT[enemy['iname']]['occurrence']=[]
                                if key not in UNIT[enemy['iname']]['occurrence']:
                                    UNIT[enemy['iname']]['occurrence'].append(key)
                            else:
                                print(enemy['iname'])

        #save
        saveAsJSON(PATH_convert2, 'Unit.json',eUnit)
        saveAsJSON(PATH_convert2, 'Enemy.json',eEnemy)
        saveAsJSON(PATH_export, 'AIO.json', master, None)


Con1=convertRaws()
Fixes(Con1)
