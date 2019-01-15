from data_pull import pull_symbols, pull_historical
from util import read_from_json_file
from datetime import datetime

def main():

    # Pull all symbols from API and write to json file
    # pull_symbols()

    # Read all symbols' data from the json file
    symbols_data = read_from_json_file('symbols')

    # Set start and end date for the historical data
    end_date = datetime.today()
    start_date = datetime(end_date.year - 5, end_date.month, end_date.day)

    # One by one, get historical data for each of the symbols and write into a separate file
    symbols = []
    for symbol_data in symbols_data[8000:]:
        symbols.append(symbol_data['symbol'])
    pull_historical(symbols, start_date, end_date)


if __name__ == '__main__':
    main()
