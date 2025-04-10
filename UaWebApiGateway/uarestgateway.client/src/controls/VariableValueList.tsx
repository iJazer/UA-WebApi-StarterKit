import React from 'react';

import TableContainer from '@mui/material/TableContainer/TableContainer';
import Table from '@mui/material/Table/Table';
import TableHead from '@mui/material/TableHead/TableHead';
import TableRow from '@mui/material/TableRow/TableRow';
import TableCell from '@mui/material/TableCell/TableCell';
import TableBody from '@mui/material/TableBody/TableBody';
import Paper from '@mui/material/Paper/Paper';
import { Typography } from '@mui/material';

import * as OpcUa from 'opcua-webapi';

import DataValueDisplay from './DataValueDisplay';
import { IMonitoredItem } from '../SubscriptionProvider';
import { SubscriptionContext } from '../SubscriptionContext';
import { HandleFactory } from '../service/HandleFactory';
import { SubscriptionState } from '../service/SubscriptionState';
import { IReadValueId } from '../service/IReadValueId';

import { BrowseContext } from '../BrowseContext';

interface VariableValueListInternals {
    internalHandle: number,
    monitoredItems: IMonitoredItem[],
    requests: number[],
    mounted: boolean,
    cleanedUp: boolean
}

interface VariableValueListProps {
    rootId?: string
    items?: AccessViewItem[];
    accessViewItems?: { displayName: string, nodeId: string, value?: OpcUa.DataValue }[];
}

interface Row {
    name: string,
    item: IMonitoredItem
}

export interface AccessViewItem {
    displayName: string;
    nodeId: string;
    value?: OpcUa.DataValue;
}

export const VariableValueList = ({ rootId, accessViewItems = [] }: VariableValueListProps) => {
    const [items, setItems] = React.useState<AccessViewItem[]>(accessViewItems);
    const [variables, setVariables] = React.useState<Row[]>([]);
    const [counter, setCounter] = React.useState<number>(1);
    

    const m = React.useRef<VariableValueListInternals>({
        internalHandle: HandleFactory.increment(),
        monitoredItems: [],
        requests: [],
        mounted: true,
        cleanedUp: false
    });

    // Set flags to trigger clean up when component is unmounted
    React.useEffect(() => {
        const state = m.current;
        return () => {
            state.mounted = false;
            state.cleanedUp = false;
        };
    }, []);

    // The hook to access active subscription.
    const {
        subscriptionState,
        subscribe,
        unsubscribe,
        unsubscribeElement,
        lastSequenceNumber
    } = React.useContext(SubscriptionContext);

    const {
        browseChildren,
        readValues,
        lastCompletedRequest,
        stateChangeCount
    } = React.useContext(BrowseContext);

    React.useEffect(() => {

        const items: IMonitoredItem[] = [];
        const newVariables: Row[] = [];

        accessViewItems.forEach((x) => {
            items.push({
                nodeId: x.nodeId,
                subscriberHandle: HandleFactory.increment()
            });
            if (x?.displayName) {
                newVariables.push({ name: x?.displayName, item: items[items.length - 1] });
            }
        });
        if (subscriptionState === SubscriptionState.Open) {
            subscribe(items, m.current.internalHandle);
            m.current.monitoredItems = items;
        }
        setVariables(newVariables);
    }, [accessViewItems]);

    const onDeleteItem = (index: number) => {
        if (subscriptionState === SubscriptionState.Open) {
            unsubscribeElement(m.current.monitoredItems, index, m.current.internalHandle);
        }
        accessViewItems.splice(index, 1);
        variables.splice(index, 1);
        setVariables(variables);
        setItems(accessViewItems);
        if (subscriptionState === SubscriptionState.Open) {
            m.current.monitoredItems = items;
        }
    };

    // Unsubscribe when component is unmounted
    React.useEffect(() => {
        const state = m.current;
        return () => {
            if (!state.mounted && !state.cleanedUp && state.monitoredItems.length) {
                unsubscribe(state.monitoredItems, state.internalHandle);
                state.cleanedUp = true;
            }
        };
    }, [unsubscribe]);

    // Handle state changes to the subscription.
    React.useEffect(() => {
        if (subscriptionState === SubscriptionState.Open) {
            const itemsToCreate = m.current.monitoredItems.filter(item => !item.itemHandle);
            if (itemsToCreate.length) {
                subscribe(itemsToCreate, m.current.internalHandle);
            }
        }
        else {
            unsubscribe(m.current.monitoredItems, m.current.internalHandle);
            m.current.monitoredItems = [];
        }
    }, [subscriptionState, subscribe, unsubscribe]);

    // Browse the root node when it changes and subscribe to the variables
    React.useEffect(() => {
        async function changeRoot() {
            const items: IMonitoredItem[] = m.current.monitoredItems;
            unsubscribe(items, m.current.internalHandle);
            m.current.monitoredItems = [];
            setVariables([]);
        }
        async function browse(nodeId: string) {
            await changeRoot();
            if (nodeId) {
                m.current.internalHandle = HandleFactory.increment();
                m.current.requests.push(m.current.internalHandle);
                console.error("browseChildren[" + nodeId + "]: " + m.current.internalHandle);
                browseChildren(m.current.internalHandle, nodeId);
            }
        }
        if (rootId) {
            browse(rootId);
        }
    }, [rootId, browseChildren, subscriptionState, unsubscribe, subscribe]);

    // Trigger render when a publish response is received.
    React.useEffect(() => {
        setCounter(counter => counter + 1);
        setItems(accessViewItems);
    }, [lastSequenceNumber]);

    React.useEffect(() => {
        if (readValues && variables.length) {
            const nodesToRead: IReadValueId[] = [];
            variables.forEach((x) => {
                if (x.item.nodeId) {
                    nodesToRead.push({
                        id: x.item.subscriberHandle ?? 0,
                        nodeId: x.item.nodeId,
                        path: x.item.path,
                        attributeId: x.item.attributeId
                    });
                }
            });
            m.current.internalHandle = HandleFactory.increment();
            m.current.requests.push(m.current.internalHandle);
            console.error("readValues[" + nodesToRead.length + "]: " + m.current.internalHandle);
            readValues(m.current.internalHandle, nodesToRead);
        }
    }, [readValues, variables, stateChangeCount]);

    React.useEffect(() => {
        console.error("ValueList[" + m.current.requests.join(",") + "]: " + (lastCompletedRequest?.callerHandle ?? 0));
        if (lastCompletedRequest && m.current.requests.find(x => x === lastCompletedRequest.callerHandle)) {
            if (lastCompletedRequest.children) {
                const items: IMonitoredItem[] = [];
                const newVariables: Row[] = [];
                lastCompletedRequest.children.forEach((x) => {
                    if (x?.reference?.NodeClass === OpcUa.NodeClass.Variable && x?.reference?.NodeId) {
                        items.push({
                            nodeId: x.reference.NodeId,
                            subscriberHandle: HandleFactory.increment()
                        });
                        if (x?.reference?.DisplayName?.Text) {
                            newVariables.push({ name: x?.reference?.DisplayName?.Text, item: items[items.length - 1] });
                        }
                    }
                });
                m.current.monitoredItems = items;
                if (subscriptionState === SubscriptionState.Open) {
                    subscribe(items, m.current.internalHandle);
                }
                console.error("setVariables[" + newVariables.length + "]: " + lastCompletedRequest.callerHandle);
                setVariables(newVariables);
            }
            else if (lastCompletedRequest.values) {
                lastCompletedRequest.values.forEach((result) => {
                    const mi = variables.find(x => x.item.subscriberHandle === result.id)
                    if (mi) {
                        mi.item.value = result.value;
                    }
                });
                console.error("setCounter[" + lastCompletedRequest.values.length + "]: " + lastCompletedRequest.callerHandle);
                // Trigger render after updating the values.
                setCounter(counter => counter + 1);
            }
            m.current.requests = m.current.requests.filter(x => x !== lastCompletedRequest.callerHandle);
        }
    }, [lastCompletedRequest, variables, subscribe, subscriptionState, setItems]);

    /*
    if (!variables?.length || counter === 0) {
        return (
            <Paper elevation={3} sx={{ height: '100%', width: 'auto', p: '4px' }} >
                <Skeleton variant='rounded' height={300}></Skeleton>
            </Paper>
        );
    }
    */
    return (
        <TableContainer component={Paper} elevation={3} sx={{ height: '100%', width: '80%' }}>
            <Table>
                <TableHead>
                    <TableRow>
                        <TableCell><Typography variant='h6'>Name</Typography></TableCell>
                        <TableCell sx={{ width: '100%' }}><Typography variant='h6'>Value</Typography></TableCell>
                    </TableRow>
                </TableHead>
                <TableBody>
                    {variables?.map((variable, index) => {
                        if (!variable) return null;
                        return (
                            <TableRow
                                key={variable.name}
                                sx={{ '&:last-child td, &:last-child th': { border: 0 } }}
                                onClick={() => onDeleteItem(index)}
                            >
                                <TableCell sx={{ width: 'auto' }}>
                                    <Typography variant='body1' sx={{ minWidth: '300px' }}>{variable.name}</Typography>
                                </TableCell>
                                <TableCell>
                                    <DataValueDisplay value={variable.item.value} />
                                </TableCell>
                            </TableRow>
                        );
                    })}
                </TableBody>
            </Table>
        </TableContainer>
    );
}

export default VariableValueList;
