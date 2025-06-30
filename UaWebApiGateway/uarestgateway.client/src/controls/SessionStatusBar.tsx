import React from 'react';
import Box from '@mui/material/Box/Box';
//import PlayArrowIcon from '@mui/icons-material/PlayArrow';
//import StopIcon from '@mui/icons-material/Stop';
import { Button, Toolbar, Typography, useTheme } from '@mui/material';

import * as OpcUa from 'opcua-webapi';
import * as Web from '../Web';
import { SessionContext } from '../SessionContext';

//import { styled } from '@mui/material/styles';
import { IMonitoredItem } from '../SubscriptionProvider';
import { SubscriptionContext } from '../SubscriptionContext';
import { HandleFactory } from '../service/HandleFactory';
import { SubscriptionState } from '../service/SubscriptionState';
import { SessionState } from '../service/SessionState';
import { IRequestMessage } from '../service/IRequestMessage';
import { TypeDefinitionCard } from './TypeDefinitionCard';

/*
const CustomButton = styled(Button)(({ theme }) => ({
   paddingLeft: 0,
   paddingRight: 0,
   minWidth: 'unset',
   '& .MuiSvgIcon-root': {
      color: theme.palette.primary.dark
   }
}));
*/

export const SessionStatusBar = () => {
    const theme = useTheme();
    const timer = React.useRef<NodeJS.Timeout | null>(null);
    const [variables, setVariables] = React.useState<IMonitoredItem[]>([]);
    const [, setCounter] = React.useState<number>(1);
    const [tickCount, setTickCount] = React.useState<number>(1);

    const {
        sessionState,
        setIsEnabled,
        setIsSessionEnabled
    } = React.useContext(SessionContext);

    const {
        setIsSubscriptionEnabled,
        subscriptionState,
        lastSequenceNumber,
    } = React.useContext(SubscriptionContext);

    React.useEffect(() => {
        const items = [{
            subscriberHandle: 1,
            nodeId: OpcUa.VariableIds.Server_ServerStatus,
            path: [OpcUa.BrowseNames.CurrentTime]
        }];
        setVariables(items);
    }, []);

    const getValue = React.useCallback((field: number) => {
        const value = variables.find(x => x.subscriberHandle === field)?.value;
        return value;
    }, [variables]);

    const valueUpdated = React.useCallback(() => {
        setCounter(counter => counter + 1);
    }, []);

    const startTimer = React.useCallback(() => {
        if (timer.current !== null) return; // Prevent starting multiple timers
        timer.current = setInterval(() => {
            setTickCount(count => count + 1);
        }, 1000);
    }, [setTickCount]);

    const stopTimer = React.useCallback(() => {
        if (timer.current !== null) {
            clearInterval(timer.current);
            timer.current = null;
        }
    }, []);

    const handleConnect = React.useCallback((state: SessionState) => {
        if (state === SessionState.Disconnected || state === SessionState.NoSession) {
            setIsSubscriptionEnabled(true);
            setIsSessionEnabled(true);
            setIsEnabled(true);
        }
        else if (state === SessionState.SessionActive || state == SessionState.Error) {
            setIsSubscriptionEnabled(false);
            setIsSessionEnabled(false);
            setIsEnabled(false);
        }
        //else if (state === SessionState.NoSession) {
        //    setIsEnabled(false);
        //}
    }, [setIsEnabled, setIsSessionEnabled, setIsSubscriptionEnabled]);

    React.useEffect(() => {
        if (sessionState === SessionState.SessionActive) {
            stopTimer();
        }
        else {
            startTimer();
        }
    }, [sessionState, startTimer, stopTimer]);

   /***
    * Handle subscription state changes
    * @param subscriptionState
    * 
    * This function is called when the subscription state changes.
    */
    const handleSubscription = React.useCallback((subscriptionState: SubscriptionState) => {
        if (subscriptionState === SubscriptionState.Closed) {
            setIsSubscriptionEnabled(true);
        }
        else {
            setIsSubscriptionEnabled(false);
        }
    }, [setIsSubscriptionEnabled]);

   return (
      <Toolbar variant='dense' disableGutters sx={{ py: 0, minHeight: '36px', justifyContent: 'space-between' }}>
         <Box ml={6} sx={{ flexGrow: 0, display: { xs: 'none', color: theme.palette.text.primary, md: 'flex' } }}>
               {/* <CustomButton sx={{ mr: 2 }} onClick={() => handleConnect(sessionState)}>
                    {(sessionState === SessionState.Disconnected) ? <PlayArrowIcon /> : <StopIcon />}
                </CustomButton> */}
            <Button sx={{ mr: 2 }} onClick={() => handleConnect(sessionState)} >
               <Typography variant='body2' sx={{ pr: 4 }}>Websocket:</Typography>
               <Typography variant='body2' fontWeight={'bolder'}>{SessionState[sessionState]}</Typography>
            </Button>
               {/* <Button sx={{ my: 2 }}>
                <TypeDefinitionCard variables={variables} onValueUpdate={valueUpdated} readTrigger={tickCount}>
                    <Typography variant='body2' sx={{ pr: 4 }}>Server Time:</Typography>
                    <Typography variant='body2' fontWeight={'bolder'}>{Web.formatTime(getValue(1)?.Value) ?? '---'}</Typography>
                </TypeDefinitionCard>
            </Button> */}
            <Button sx={{ my: 2 }} onClick={() => handleSubscription(subscriptionState)}>
               <Typography variant='body2' sx={{ pr: 4 }}>Subscription:</Typography>
               <Typography variant='body2' fontWeight={'bolder'}>{SubscriptionState[subscriptionState]}</Typography>
            </Button>
            <Button sx={{ my: 2 }}>
               <Typography variant='body2' sx={{ pr: 4 }}>Last Publish:</Typography>
               <Typography variant='body2' fontWeight={'bolder'}>{lastSequenceNumber ?? '---'}</Typography>
            </Button>
         </Box>
      </Toolbar>
   );
}

export default SessionStatusBar;