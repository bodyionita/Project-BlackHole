from iexfinance import get_available_symbols
from iexfinance.stocks import get_historical_data, Stock
from iexfinance.utils.exceptions import *

from util import write_to_json_file
from datetime import datetime


def pull_symbols():
    """

    :return:
    """

    symbols = get_available_symbols()

    write_to_json_file(data=symbols, filename='symbols')


def pull_symbols_extended():
    """

    :return:
    """
    symbols = get_available_symbols()

    symbols_extended = []
    for symbol in symbols:
        stock = Stock(symbol['symbol'], output_format='json')
        symbol_extended = stock.get_company()
        symbols_extended.append(symbol_extended)

    write_to_json_file(data=symbols_extended, filename='symbols_extended')


def pull_historical(symbols, date_start=None, date_end=None):
    """

    :param symbols: [string] -> list of all the symbols to get data for
    :param date_start: datetime -> start date of the historical data
    :param date_end: datetime -> end date of the historical data
    :return:
    """
    if (date_start is None) and (date_end is None):
        date_end = datetime.today()
        date_start = datetime(date_end.year - 1, date_end.month, date_end.day)

    for symbol in symbols:
        try:
            symbol_data = get_historical_data(symbol, date_start, date_end, output_format='json')
        except IEXSymbolError as e:
            with open('data/symbols_not_found.txt', 'a') as outfile:
                outfile.write(str(e)+'\n')
        else:
            write_to_json_file(data=symbol_data, filename=symbol, subdir='symbol_data/')

